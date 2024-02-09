
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherReportAPI.Models.Entities;
using XPlot.Plotly;

namespace WeatherReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherGraphController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherGraphController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeatherGraph()
        {
            
            var apiKey = "http://api.weatherapi.com/v1/history.json?key=4f4565dd540f4bf192c111401240902";
            var city = "Lagos";
            var days = 14;

            var temperatures = await FetchTemperatureData(apiKey, city, days);

            var dates = new List<DateTime>();
            var values = new List<decimal>();

            foreach (var temperature in temperatures)
            {
                dates.Add(temperature.Key);
                values.Add(temperature.Value.temp_c);
            }

            var scatter = new Scattergl
            {
                x = dates,
                y = values,
                mode = "lines+markers",
                name = "Temperature in Lagos"
            };

            var chart = Chart.Plot(scatter);
            chart.WithTitle($"Temperature in {city} over a {days}-day period");
            chart.WithXTitle("Date");
            chart.WithYTitle("Temperature (°C)");

            chart.Width = 800;
            chart.Height = 600;

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = 200,
                Content = chart.GetHtml()
            };
        }
        
        private async Task<Dictionary<DateTime, Current>> FetchTemperatureData(string apiKey, string city, int days)
            {
            
            
                var temperatureData = new Dictionary<DateTime, Current>();

                using var httpClient = _httpClientFactory.CreateClient();

                for (int i = 0; i < days; i++)
                {
                    var date = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd");
                    var response = await httpClient.GetAsync($"https://api.weatherapi.com/v1/history.json?key={apiKey}&q={city}&dt={date}");
                    response.EnsureSuccessStatusCode();

                    var weather = await JsonSerializer.DeserializeAsync<Weather>(await response.Content.ReadAsStreamAsync());

                    if (weather.Error != null)
                    {
                        Console.WriteLine($"Error fetching data for {date}: {weather.Error.Message}");
                        continue;
                    }

                    temperatureData[DateTime.Parse(date)] = weather.Current;
                }

                return temperatureData;
            }
        }
    }
}