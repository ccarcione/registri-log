using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using net_registri_log.ApiLog.Models;
using net_registri_log.AuditLog.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_registri_log
{
    public class RegistriLogDbContext : DbContext
    {
        private Dictionary<AuditEntry, EntityEntry> _auditWithoutKeyValues = new Dictionary<AuditEntry, EntityEntry>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RegistriLogDbContext(DbContextOptions<RegistriLogDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Ignore<Log>();
        }

        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<ApiObject> ApiLogs { get; set; }
        //public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// SaveChangesAsync con sistema audit trail.
        /// </summary>
        /// <param name="userId">se non impostato tento di recuperarlo dai Claims.</param>
        /// <param name="userName">se non impostato tento di recuperarlo dai Claims.</param>
        /// <returns></returns>
        public async Task SaveAndAuditChangesAsync(DbContext applicationDbContextProject, string userId = null, string userName = null)
        {
            userId = userId ?? _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            userName = userName ?? string.Concat(
                _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value,
                " ",
                _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value);
            OnBeforeSaveChanges(applicationDbContextProject, userId, userName);
            await applicationDbContextProject.SaveChangesAsync();

            // restore EntityEntry Without Id
            foreach (var audiEntityEntry in _auditWithoutKeyValues)
            {
                var newKeyValues = new Dictionary<string, object>();
                var audit = audiEntityEntry.Key.Audit;
                var entityEntry = audiEntityEntry.Value;

                // update audit.PrimaryKey field
                foreach (var property in entityEntry.Properties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        newKeyValues.Add(property.Metadata.Name, property.CurrentValue);
                        continue;
                    }
                }

                // UpdatePrimaryKey
                audit.PrimaryKey = JsonConvert.SerializeObject(newKeyValues);
            }

            await this.SaveChangesAsync();
        }
        private void OnBeforeSaveChanges(DbContext dbContext, string userId, string userName)
        {
            dbContext.ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                bool isRemoved = false;
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntry.UserName = userName;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        // the "property.CurrentValue" value is temporary because the id is resolved by sql only after query insert
                        // then I trace the new entities so as to update the KeyValues audit field with the correct KVal.
                        _auditWithoutKeyValues[auditEntry] = entry;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditLog.Models.Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditLog.Models.Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                if (propertyName.Equals("IsRemoved", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isRemoved = bool.Parse(property.CurrentValue.ToString());
                                }
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditLog.Models.Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }

                // if the object has been set removed, the operation status must be Removed
                if (isRemoved)
                {
                    auditEntry.AuditType = AuditLog.Models.Enums.AuditType.Removed;
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}
