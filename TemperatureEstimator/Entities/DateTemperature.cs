using System;

namespace TemperatureEstimator.Entities
{
    public class DateTemperature
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public string Airport { get; set; }
    }
}