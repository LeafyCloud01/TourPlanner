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

            var logAMDControls = new AMDControlsViewModel();
            var logInfoDisplay = new LogInfoDisplayViewModel();
            var tourInfoDisplay = new TourInfoDisplayViewModel();
            var routeInfoDisplay = new RouteInfoDisplayViewModel();
            var tourInputs = new TourInputsViewModel();
            var logInputs = new LogInputsViewModel();

            var tourAMDSelectionList = new AMDSelectionListViewModel(tourSearchBox, tourAMDControls, tourSelectionList);
            var tourDisplay = new TourDisplayViewModel(tourInfoDisplay, routeInfoDisplay, tourInputs, logInfoDisplay, logInputs, logAMDControls);

            var window = new MainWindow
            {
                DataContext = new MainViewModel(tourAMDSelectionList, tourDisplay),
                TourAMDSelectionList = { DataContext = tourAMDSelectionList }, 
                DisplayTour = { DataContext = tourDisplay }
            };

            window.Show();
        }


    }

}
