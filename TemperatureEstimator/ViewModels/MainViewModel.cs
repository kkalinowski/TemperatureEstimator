using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using System.Collections.Generic;
using TemperatureEstimator.Entities;
using TemperatureEstimator.EstimationEngines;
using TemperatureEstimator.Logic;
using TemperatureEstimator.Properties;

namespace TemperatureEstimator.ViewModels
{
    [Singleton]
    public class MainViewModel : NotifyingObject
    {
        #region Properties
        [WireUp]
        public DataManager DataManager { get; set; }
        [WireUp]
        public WeightedMeanEngine WeightedMeanEngine { get; set; }
        [WireUp]
        public ArmaEngine ArmaEngine { get; set; }
        [WireUp]
        public NeuronNetworkEngine NeuronNetworkEngine { get; set; }

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

        private double weightedMeanEstimation;
        public double WeightedMeanEstimation
        {
            get { return weightedMeanEstimation; }
            set
            {
                weightedMeanEstimation = value;
                OnPropertyChanged("WeightedMeanEstimation");
            }
        }

        private double armaEstimation;
        public double ArmaEstimation
        {
            get { return armaEstimation; }
            set
            {
                armaEstimation = value;
                OnPropertyChanged("ArmaEstimation");
            }
        }

        private double neuronNetworkEstimation;
        public double NeuronNetworkEstimation
        {
            get { return neuronNetworkEstimation; }
            set
            {
                neuronNetworkEstimation = value;
                OnPropertyChanged("NeuronNetworkEstimation");
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public ICommand LoadedCommand { get; private set; }
        #endregion

        public MainViewModel()
        {
            IsLoading = true;
            LoadedCommand = new DelegateCommand(x => Task.Run(() => Load()));
        }

        private void Load()
        {
            DataManager.Load(Settings.Default.Airport);
            Data = DataManager.Data.Where(x => x.Date >= DateTime.Today.AddMonths(-2)).ToList();

            TodaysTemperature = Data.Last().Value;
            WeightedMeanEstimation = WeightedMeanEngine.Estimate(DataManager.Data);
            ArmaEstimation = ArmaEngine.Estimate(Data);
            NeuronNetworkEstimation = NeuronNetworkEngine.Estimate(DataManager.Data);

            IsLoading = false;
        }
    }
}