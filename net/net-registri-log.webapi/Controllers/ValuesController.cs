using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace net_registri_log.webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly RegistriLogDbContext registriLogDbContext;
        private readonly ApplicationDbContext applicationDbContext;

        public ValuesController(ILogger<ValuesController> logger,
            RegistriLogDbContext registriLogDbContext,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            this.registriLogDbContext = registriLogDbContext;
            this.applicationDbContext = applicationDbContext;
        }

        [HttpGet("crea")]
        public async Task<IActionResult> create(string summary)
        {
            WeatherForecast weatherForecast = new WeatherForecast()
            {
                Summary = summary
            };
            applicationDbContext.WeatherForecast.Add(weatherForecast);


            //await applicationDbContext.SaveChangesAsync();
            await registriLogDbContext.SaveAndAuditChangesAsync(applicationDbContext);
            return Ok();
        }
    }
}
