using lib12.Collections;
using lib12.DependencyInjection;
using System.Windows;
using TemperatureEstimator.Properties;
using TemperatureEstimator.Views;

namespace TemperatureEstimator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Instances.RegisterTransient<LocationChoice>();
            Instances.RegisterTransient<MainWindow>();

            if(Settings.Default.Airport.IsNullOrEmpty())
                Instances.Get<LocationChoice>().Show();
            else
                Instances.Get<MainWindow>().Show();
        }
    }
}
