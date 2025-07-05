using BusinessLayer;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace PresentationLayer.ViewModels
{
    public class MainViewModel
    {
        public TourList tourList = BusinessManager.GetTourListDb();

        public readonly AMDSelectionListViewModel tourAMDSelectionList;
        public readonly TourDisplayViewModel tourDisplay;

        public MainViewModel(AMDSelectionListViewModel tourAMDSelectionList, TourDisplayViewModel tourDisplay)
        {
            this.tourAMDSelectionList = tourAMDSelectionList;
            this.tourDisplay = tourDisplay;
        }
    }
}
