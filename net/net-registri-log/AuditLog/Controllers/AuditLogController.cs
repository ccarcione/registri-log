using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using net_registri_log.AuditLog.Models;
using net_registri_log.Shared.Models;
using System.Linq;
using System.Threading.Tasks;
using net_registri_log.Shared.ExtensionMethods;
using net_registri_log.AuditLog.Models.Enums;

namespace net_registri_log.AuditLog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class AuditLogController : ControllerBase
    {
        private readonly RegistriLogDbContext _context;
        private readonly ILogger<AuditLogController> _logger;

        public AuditLogController(RegistriLogDbContext context, ILogger<AuditLogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("GetPaginazione")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters, [FromBody] FiltriAuditLog filtri)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            //_logger.LogDebugPushProperty("Sono state richieste le schedeTecniche con i seguenti filtri.", data: filtri);

            IQueryable<Audit> data = _context.AuditLogs
                //.Where(c => filtri.DataDa.HasValue ? c.Data.Date >= filtri.DataDa.Value.Date : true)
                //.Where(c => filtri.DataA.HasValue ? c.Data.Date <= filtri.DataA.Value.Date : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.UserId) ? c.UserId.Equals(filtri.UserId) : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.Type) ? c.Type.Equals(filtri.Type) : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.TableName) ? c.TableName.Equals(filtri.TableName) : true)
                ;

            switch (filtri.OrderColumn.ToEnum<FiltriAuditLogEnum>())
            {
                case FiltriAuditLogEnum.Data:
                    data = filtri.Desc ? data.OrderByDescending(c => c.DateTime) : data.OrderBy(c => c.DateTime);
                    break;
                case FiltriAuditLogEnum.Id:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(c => c.Id);
                    break;
                default:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(c => c.Id);
                    break;
            }

            PagedList<Audit> pagedList = PagedList<Audit>.ToPagedList(data, queryParameters);
            _logger.LogDebug($"Returned {pagedList.Data.Count()} Audit items.");

            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Audit audit = await _context.AuditLogs.SingleAsync(s => s.Id == id);

            if (audit == null)
            {
                return NotFound();
            }

            return Ok(audit);
        }
    }
}
