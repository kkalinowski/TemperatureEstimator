using System.Windows.Input;
using lib12.DependencyInjection;
using lib12.WPF.Core;
using MahApps.Metro.Controls;
using TemperatureEstimator.Entities;
using TemperatureEstimator.Properties;

namespace TemperatureEstimator.ViewModels
{
    [Singleton]
    public class LocationChoiceViewModel : NotifyingObject
    {
        #region Props
        private Location[] locations;
        public Location[] Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged("Locations");
            }
        }

        private Location selectedLocation;
        public Location SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        public ICommand OkCommand { get; private set; }
        #endregion

        public LocationChoiceViewModel()
        {
            OkCommand = new DelegateCommand(ExecuteOk, CanExecuteOk);
            Locations = AvailableLocations.Locations;
        }

        private void ExecuteOk(object parameter)
        {
            Settings.Default.Airport = SelectedLocation.AirportCode;
            Settings.Default.City = SelectedLocation.City;
            Settings.Default.Save();

            Instances.Get<MainWindow>().Show();
            ((MetroWindow)parameter).Close();
        }

        private bool CanExecuteOk(object parameter)
        {
            return SelectedLocation != null;
        }
    }
}