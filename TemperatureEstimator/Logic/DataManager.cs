using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using lib12.DependencyInjection;
using SqlFu;
using TemperatureEstimator.Entities;

namespace TemperatureEstimator.Logic
{
    [Singleton]
    public class DataManager
    {
        public List<DateTemperature> Data { get; set; }

        public void Load()
        {
            CheckDb();
            LoadDataFromDb();
        }

        private void CheckDb()
        {
            using (var db = SqlFuDao.GetConnection())
            {
                if(!db.TableExists<DateTemperature>())
                    db.CreateTable<DateTemperature>();
            }
        }

        private void LoadDataFromDb()
        {
            using (var db = SqlFuDao.GetConnection())
            {
                Data = db.Query<DateTemperature>().ToList();
            }
        }
    }
}