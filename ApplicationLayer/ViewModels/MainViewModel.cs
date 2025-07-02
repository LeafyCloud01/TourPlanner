using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace PresentationLayer.ViewModels
{
    public class MainViewModel
    {
        public TourList tourList = BusinessManager.GetTourList();

        public readonly AMDSelectionListViewModel tourAMDSelectionList;
        public readonly TourDisplayViewModel tourDisplay;

        public MainViewModel(AMDSelectionListViewModel tourAMDSelectionList, TourDisplayViewModel tourDisplay)
        {
            this.tourAMDSelectionList = tourAMDSelectionList;
            this.tourDisplay = tourDisplay;
        }

        private void UpdateCurrentTour()
        {
            MessageBox.Show("Sending: \n", JsonSerializer.Serialize<Tour>(tourList.tours[0]));
            Messenger.Default.Send<Tour>(tourList.tours[0]);
        }
    }
}
