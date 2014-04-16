using CsvHelper.Configuration;

namespace TemperatureEstimator.Entities
{
    public class DateTemperatureMap : CsvClassMap<DateTemperature>
    {
        public DateTemperatureMap()
        {
            Map(x => x.Id).Ignore();
            Map(x => x.Date).Index(0);
            Map(x => x.Temperature).Index(1);
            Map(x => x.Airport).Ignore();
        }
    }
}