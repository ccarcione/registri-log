using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using net_registri_log.ApiLog.Models;
using net_registri_log.AuditLog.Models.Enums;
using net_registri_log.Shared.ExtensionMethods;
using net_registri_log.Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace net_registri_log.ApiLog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiLogController : ControllerBase
    {
        private readonly RegistriLogDbContext _context;
        private readonly ILogger<ApiLogController> _logger;

        public ApiLogController(RegistriLogDbContext context, ILogger<ApiLogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("GetPaginazione")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters, [FromBody] FiltriApiLog filtri)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            IQueryable<ApiObject> data = _context.ApiLogs
                //.Where(c => filtri.DataDa.HasValue ? c.Data.Date >= filtri.DataDa.Value.Date : true)
                //.Where(c => filtri.DataA.HasValue ? c.Data.Date <= filtri.DataA.Value.Date : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.UserId) ? c.UserId.Equals(filtri.UserId) : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.Method) ? c.Method.Equals(filtri.Method) : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.Url) ? c.Url.StartsWith(filtri.Url) : true)
                .Where(c => !string.IsNullOrWhiteSpace(filtri.QueryString) ? c.QueryString.Contains(filtri.QueryString) : true)
                ;

            switch (filtri.OrderColumn.ToEnum<FiltriApiLogEnum>())
            {
                case FiltriApiLogEnum.Data:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Date) : data.OrderBy(nc => nc.Date);
                    break;
                case FiltriApiLogEnum.Id:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(nc => nc.Id);
                    break;
                default:
                    data = filtri.Desc ? data.OrderByDescending(c => c.Id) : data.OrderBy(nc => nc.Id);
                    break;
            }

            PagedList<ApiObject> pagedList = PagedList<ApiObject>.ToPagedList(data, queryParameters);
            _logger.LogDebug($"Returned {pagedList.Data.Count()} ApiObject items.");

            return Ok(pagedList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ApiObject apiObject = await _context.ApiLogs.SingleAsync(s => s.Id == id);

            if (apiObject== null)
            {
                return NotFound();
            }

            return Ok(apiObject);
        }
    }
}
