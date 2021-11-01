using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
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

        [HttpGet("new")]
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

        [HttpGet("edit")]
        public async Task<IActionResult> edit(int id, string summary)
        {
            WeatherForecast weatherForecast = applicationDbContext.WeatherForecast
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            weatherForecast.Summary = summary;
            WeatherForecast old = applicationDbContext.WeatherForecast.Find(id);
            applicationDbContext.Entry(old).CurrentValues.SetValues(weatherForecast);

            //await applicationDbContext.SaveChangesAsync();
            await registriLogDbContext.SaveAndAuditChangesAsync(applicationDbContext);
            return Ok();
        }

        [HttpGet("remove")]
        public async Task<IActionResult> remove(int id)
        {
            WeatherForecast weatherForecast = applicationDbContext.WeatherForecast
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            weatherForecast.IsRemoved = true;
            WeatherForecast old = applicationDbContext.WeatherForecast.Find(id);
            applicationDbContext.Entry(old).CurrentValues.SetValues(weatherForecast);

            //await applicationDbContext.SaveChangesAsync();
            await registriLogDbContext.SaveAndAuditChangesAsync(applicationDbContext);
            return Ok();
        }

        [HttpGet("delete")]
        public async Task<IActionResult> delete(int id)
        {
            WeatherForecast weatherForecast = applicationDbContext.WeatherForecast
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            applicationDbContext.WeatherForecast.Remove(weatherForecast);

            //await applicationDbContext.SaveChangesAsync();
            await registriLogDbContext.SaveAndAuditChangesAsync(applicationDbContext);
            return Ok();
        }

    }
}
