namespace TemperatureEstimator.Entities
{
    public class AvailableLocations
    {
        public static Location[] Locations { get; private set; }

        static AvailableLocations()
        {
            Locations = new[]
            {
                new Location
                {
                    AirportCode = "EPKK",
                    City = "Kraków"
                },
                new Location
                {
                    AirportCode = "EPWA",
                    City = "Warszawa"
                },
                new Location
                {
                    AirportCode = "EPWR",
                    City = "Wrocław"
                },
                new Location
                {
                    AirportCode = "EPPO",
                    City = "Poznań"
                },
                new Location
                {
                    AirportCode = "CYXU",
                    City = "London"
                },
                new Location
                {
                    AirportCode = "LFPG",
                    City = "Paris"
                },
                new Location
                {
                    AirportCode = "KLGA",
                    City = "New York"
                },
            };
        }
    }
}