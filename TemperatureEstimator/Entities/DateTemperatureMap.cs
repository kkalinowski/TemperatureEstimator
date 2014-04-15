using CsvHelper.Configuration;

namespace TemperatureEstimator.Entities
{
    public class DateTemperatureMap : CsvClassMap<DateTemperature>
    {
        public DateTemperatureMap()
        {
            Map(x => x.Date).Index(0);
            Map(x => x.Temperature).Index(1);
        }
    }
}