using System;

namespace TemperatureEstimator.Entities
{
    public interface IDateValue
    {
        DateTime Date { get; set; }
        double Value { get; set; }
    }
}