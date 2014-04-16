using CsvHelper;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using System;
using System.Collections.Generic;
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

        public List<DateTemperature> DownloadTemperature(string airport, DateTime lastDate)
        {
            var list = new List<DateTemperature>();
            using (var client = new WebClient())
            {
                var date = lastDate;
                var today = DateTime.Today;
                while (date <= today)
                {
                    var stream = OpenWeatherStream(client, airport, date);
                    list.AddRange(ParseWeatherStream(stream));
                    date = date.AddMonths(1);
                }
            }

            return list;
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