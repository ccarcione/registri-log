using Microsoft.EntityFrameworkCore.ChangeTracking;
using net_registri_log.AuditLog.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace net_registri_log.AuditLog.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        private Audit _audit;

        public Audit Audit
        {
            get { return _audit; }
            private set { _audit = value; }
        }

        public Audit ToAudit()
        {
            Audit = new Audit();
            Audit.UserId = UserId;
            Audit.UserName = UserName;
            Audit.Type = AuditType.ToString();
            Audit.TableName = TableName;
            Audit.DateTime = DateTime.Now;
            Audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            Audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            Audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            Audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return Audit;
        }
    }
}
