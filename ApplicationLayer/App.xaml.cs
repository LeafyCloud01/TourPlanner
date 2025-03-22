using System.Configuration;
using System.Data;
using System.Windows;

using PresentationLayer.ViewModels;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var tourSearchBox = new SearchBoxViewModel();
            var tourAMDControls = new AMDControlsViewModel();
            var tourSelectionList = new SelectionListViewModel();
            var tourAMDSelectionList = new AMDSelectionListViewModel();
            var tourDisplay = new TourDisplayViewModel();

            var window = new MainWindow
            {
                DataContext = new MainViewModel(),
                TourAMDSelectionList = { DataContext = tourAMDSelectionList }, 
                DisplayTour = { DataContext = tourDisplay }
            };

            window.Show();
        }


    }

}
