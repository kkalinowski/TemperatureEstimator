using System;
using System.Linq;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using System.Collections.Generic;
using TemperatureEstimator.Entities;
using TemperatureEstimator.Logic;
using TemperatureEstimator.Properties;

namespace TemperatureEstimator.ViewModels
{
    [Singleton]
    public class MainViewModel : NotifyingObject
    {
        #region Properties
        private List<DateTemperature> data;
        public List<DateTemperature> Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        private double todaysTemperature;
        public double TodaysTemperature
        {
            get { return todaysTemperature; }
            set
            {
                todaysTemperature = value;
                OnPropertyChanged("TodaysTemperature");
            }
        }
        #endregion

        public MainViewModel(DataManager dataManager)
        {
            dataManager.Load(Settings.Default.Airport);
            Data = dataManager.Data.Where(x => x.Date >= DateTime.Today.AddMonths(-2)).ToList();
            TodaysTemperature = Data.Last().Temperature;
        }
    }
}