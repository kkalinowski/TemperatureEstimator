using CsvHelper;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using System;
using System.IO;
using System.Linq;
using System.Net;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.Logic
{
    [Singleton]
    public class DataDownloader : NotifyingObject
    {
        #region Const
        private const string Query = @"http://www.wunderground.com/history/airport/{0}/{1}/{2}/{3}/MonthlyHistory.html?format=1";
        #endregion

        public DateTemperature[] DownloadTemperature(string airport, DateTime lastDate)
        {
            using (var client = new WebClient())
            {
                var stream = OpenWeatherStream(client, airport, lastDate);
                return ParseWeatherStream(stream);
            }
        }

        private Stream OpenWeatherStream(WebClient client, string airport, DateTime date)
        {
            var queryString = string.Format(Query, airport, date.Year, date.Month, date.Day);
            return client.OpenRead(queryString);
        }

        private DateTemperature[] ParseWeatherStream(Stream weatherStream)
        {
            var reader = new StreamReader(weatherStream);
            var csvReader = new CsvReader(reader);
            csvReader.Configuration.RegisterClassMap<DateTemperatureMap>();

            return csvReader.GetRecords<DateTemperature>().ToArray();
        }
    }
}