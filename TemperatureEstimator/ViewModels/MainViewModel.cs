using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using lib12.Core;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using lib12.WPF.EventTranscriptions;
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

        private List<EstimationResult> results;
        public List<EstimationResult> Results
        {
            get { return results; }
            set
            {
                results = value;
                OnPropertyChanged("Results");
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
        public ICommand InfoCommand { get; private set; }
        public ICommand AppInfoCommand { get; private set; }
        #endregion

        public MainViewModel()
        {
            IsLoading = true;
            LoadedCommand = new DelegateCommand(x => Task.Run(() => Load()));
            InfoCommand = new DelegateCommand<EventTranscriptionParameter<RequestNavigateEventArgs>>(ExecuteInfo);
            AppInfoCommand = new DelegateCommand(ExecuteAppInfoCommand);
        }

        private List<IEstimationEngine> LoadAlgorithms()
        {
            return new List<IEstimationEngine>
            {
                Instances.Get<WeightedMeanEngine>(),
                Instances.Get<TrendEngine>(),
                Instances.Get<ArmaEngine>(),
                Instances.Get<NeuronNetworkEngine>()
            };
        }

        private void Load()
        {
            DataManager.Load(Settings.Default.Airport);
            var algorithms = LoadAlgorithms();
            Data = DataManager.Data.Where(x => x.Date >= DateTime.Today.AddMonths(-2)).ToList();
            TodaysTemperature = Data.Last().Value;

            var result = algorithms.Select(x => x.Estimate(Data)).ToList();
            ComputeErrors(result);
            Results = result;

            SaveEstimation();
            IsLoading = false;
        }

        private void ComputeErrors(IEnumerable<EstimationResult> result)
        {
            var weightedMeanError = 0.0;
            var trendError = 0.0;
            var armaError = 0.0;
            var neuronNetworkError = 0.0;
            var estimatesCount = 0;

            for (int i = 0; i < Data.Count - 1; i++)
            {
                if (Data[i].WeightedMeanEstimation.HasValue)
                {
                    weightedMeanError += Math.Abs(Data[i].WeightedMeanEstimation.Value - Data[i + 1].Value);
                    trendError += Math.Abs(Data[i].TrendEstimation.Value - Data[i + 1].Value);
                    armaError += Math.Abs(Data[i].ArmaEstimation.Value - Data[i + 1].Value);
                    neuronNetworkError += Math.Abs(Data[i].NeuronNetworkEstimation.Value - Data[i + 1].Value);
                    estimatesCount++;
                }
            }

            result.First(x => x.Estimator == Estimator.WeightedMean).Error = Math2.DivWithZero(weightedMeanError, estimatesCount);
            result.First(x => x.Estimator == Estimator.Trend).Error = Math2.DivWithZero(trendError, estimatesCount);
            result.First(x => x.Estimator == Estimator.NeuronNetwork).Error = Math2.DivWithZero(neuronNetworkError, estimatesCount);
            result.First(x => x.Estimator == Estimator.ARMA).Error = Math2.DivWithZero(armaError, estimatesCount);
        }

        private void SaveEstimation()
        {
            var todaysData = Data.Last();
            todaysData.WeightedMeanEstimation = Results.First(x => x.Estimator == Estimator.WeightedMean).Value;
            todaysData.TrendEstimation = Results.First(x => x.Estimator == Estimator.Trend).Value;
            todaysData.NeuronNetworkEstimation = Results.First(x => x.Estimator == Estimator.NeuronNetwork).Value;
            todaysData.ArmaEstimation = Results.First(x => x.Estimator == Estimator.ARMA).Value;

            DataManager.Save(todaysData);
        }

        private void ExecuteInfo(EventTranscriptionParameter<RequestNavigateEventArgs> e)
        {
            Process.Start(new ProcessStartInfo(e.EventArgs.Uri.AbsoluteUri));
            e.EventArgs.Handled = true;
        }

        private void ExecuteAppInfoCommand(object obj)
        {
            MessageBox.Show("Application created by Krzysztof Kalinowski - kkalinowski.net \nDownloads temperature from previous days and uses various algorithms to predicts future weather.",
                "Temperature Estimator - Info");
        }
    }
}