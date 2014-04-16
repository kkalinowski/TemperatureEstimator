﻿using lib12.DependencyInjection;
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

        public MainViewModel(DataManager dataManager)
        {
            dataManager.Load(Settings.Default.Airport);
            Data = dataManager.Data;
        }
    }
}