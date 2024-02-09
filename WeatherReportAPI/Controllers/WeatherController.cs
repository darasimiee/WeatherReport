using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherReportAPI.Models.Entities;


namespace WeatherReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
       // private readonly IHttpClientFactory _httpClientFactory;

        public WeatherController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            //_httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetCurrentWeatherForecast")]
        public async Task<WeatherForecast> Get([FromQuery] string city)
        {
            var client = new HttpClient();
            var result = await client.GetFromJsonAsync<WeatherForecast>($"http://api.weatherapi.com/v1/current.json?key=4f4565dd540f4bf192c111401240902&q={city}");

            return result;
        }

        [HttpGet("/GetHistory")]
        public async Task<IEnumerable<WeatherForecast>> GetHistory([FromQuery] string city, [FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                var client = new HttpClient();
                var result = await client.GetFromJsonAsync<List<WeatherForecast>>($"http://api.weatherapi.com/v1/history.json?key=4f4565dd540f4bf192c111401240902&q={city}&dt={startDate}&end_dt={endDate}");

                return result;
            }
            catch (Exception ex)
            {
                return new List<WeatherForecast>();
                Console.WriteLine($"Subscribe {ex.Message}");
            }

        }

        
    }
}



