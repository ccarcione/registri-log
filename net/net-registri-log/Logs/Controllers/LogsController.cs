using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using net_registri_log.Shared.Models;
using System.Linq;
using System.Threading.Tasks;
using net_registri_log.Shared.ExtensionMethods;
using net_registri_log.Logs.Models;

namespace net_registri_log.Logs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class LogsController : ControllerBase
    {
        private readonly RegistriLogDbContext _context;
        private readonly ILogger<LogsController> _logger;

        public LogsController(RegistriLogDbContext context, ILogger<LogsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("GetPaginazione")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters, [FromBody] FiltriLogs filtri)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            IQueryable<Log> data = _context.Logs
                //.Where(c => filtri.DataDa.HasValue ? c.Data.Date >= filtri.DataDa.Value.Date : true)
                //.Where(c => filtri.DataA.HasValue ? c.Data.Date <= filtri.DataA.Value.Date : true)
                //.Where(c => !string.IsNullOrWhiteSpace(filtri.TableName) ? c.TableName.Equals(filtri.TableName) : true)
                ;

            switch (filtri.OrderColumn.ToEnum<FiltriLogsEnum>())
            {
                case FiltriLogsEnum.Timestamp:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Timestamp) : data.OrderBy(c => c.Timestamp);
                    break;
                case FiltriLogsEnum.Id:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(c => c.Id);
                    break;
                default:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(c => c.Id);
                    break;
            }

            PagedList<Log> pagedList = PagedList<Log>.ToPagedList(data, queryParameters);
            _logger.LogDebug($"Ritornati {pagedList.Data.Count()} elementi Logs.");

            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Log log = await _context.Logs.SingleAsync(s => s.Id == id);

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }
    }
}
