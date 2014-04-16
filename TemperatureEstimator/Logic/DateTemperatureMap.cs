using CsvHelper.Configuration;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.Logic
{
    public class DateTemperatureMap : CsvClassMap<DateTemperature>
    {
        public DateTemperatureMap()
        {
            Map(x => x.Date).Index(0);
            Map(x => x.Value).Index(1).TypeConverter<FailSafeCsvDoubleTypeConverter>();
            Map(x => x.Id).Ignore();
            Map(x => x.Airport).Ignore();
        }
    }
}