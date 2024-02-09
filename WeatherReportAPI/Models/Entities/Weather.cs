namespace WeatherReportAPI.Models.Entities
{
    public class Weather
    {
         
		public Location Location { get; set; }

        public Current Current { get; set; }

        public Error Error { get; set; }
    }

    public class Location
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }

    public class Current
    {
        public decimal temp_c { get; set; }
        public decimal Humidity { get; set; }
        public int Cloud { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
