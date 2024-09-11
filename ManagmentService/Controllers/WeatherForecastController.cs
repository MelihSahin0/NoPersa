using ManagmentService.Database;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private NoPersaDbContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, NoPersaDbContext noPersaDbContext)
        {
            _context = noPersaDbContext;
            _logger = logger;
        }


        [HttpGet("GetWeatherForecast", Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetWeatherForecastDb", Name = "GetWeatherForecastDb")]
        public IEnumerable<WeatherForecast> GetDb()
        {
            _context.WeatherForecasts.Add(new WeatherForecast() { Date = DateOnly.FromDateTime(DateTime.Today), Summary = "e", TemperatureC = 200 });
            _context.SaveChanges();

            return [.. _context.WeatherForecasts];
        }
    }
}
