using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using log4net;
using log4net.Config;
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
            XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), new FileInfo("log4net.config"));

            var tourSearchBox = new SearchBoxViewModel();
            var tourAMDControls = new AMDControlsViewModel();
            var tourSelectionList = new SelectionListViewModel();

            var logAMDControls = new AMDControlsViewModel();
            var logInfoDisplay = new LogInfoDisplayViewModel();
            var tourInfoDisplay = new TourInfoDisplayViewModel();
            var routeInfoDisplay = new RouteInfoDisplayViewModel();
            var tourInputs = new TourInputsViewModel();
            var logInputs = new LogInputsViewModel();
            var deleteTour = new DeleteTourViewModel();

            var tourAMDSelectionList = new AMDSelectionListViewModel(tourSearchBox, tourAMDControls, tourSelectionList);
            var tourDisplay = new TourDisplayViewModel(tourInfoDisplay, routeInfoDisplay, tourInputs, logInfoDisplay, logInputs, logAMDControls, deleteTour);

            var mainViewModel = new MainViewModel(tourAMDSelectionList, tourDisplay);

            var window = new MainWindow
            {
                DataContext = mainViewModel,
                TourAMDSelectionList = { DataContext = mainViewModel.tourAMDSelectionList }, 
                DisplayTour = { DataContext = mainViewModel.tourDisplay }
            };

            window.Show();
        }

    }

}