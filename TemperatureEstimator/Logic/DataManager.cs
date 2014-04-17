using lib12.DependencyInjection;
using SqlFu;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.Logic
{
    [Singleton]
    public class DataManager
    {
        [WireUp]
        public DataDownloader DataDownloader { get; set; }

        public List<DateTemperature> Data { get; set; }

        public void Load(string airport)
        {
            using (var db = SqlFuDao.GetConnection())
            {
                CheckDb(db);
                LoadDataFromDb(db);
            }

            DownloadMissingData(airport);
        }

        private void CheckDb(DbConnection db)
        {
            if (!db.TableExists<DateTemperature>())
                db.CreateTable<DateTemperature>();
        }

        private void LoadDataFromDb(DbConnection db)
        {
            Data = db.Query<DateTemperature>().ToList();
        }

        private void DownloadMissingData(string airport)
        {
            var lastMeasure = Data.Any() ? Data.Max(x => x.Date) : DateTime.Today.AddYears(-1);
            if (lastMeasure.Date == DateTime.Today)
                return;

            var missingData = DataDownloader.DownloadTemperature(airport, lastMeasure);
            using (var db = SqlFuDao.GetConnection())
            {
                missingData.ForEach(x =>
                {
                    x.Airport = airport;
                    db.Insert(x);
                });
            }

            Data.AddRange(missingData);
        }

        public void Save(DateTemperature data)
        {
            using (var db = SqlFuDao.GetConnection())
            {
                db.Update<DateTemperature>(data);
            }
        }
    }
}