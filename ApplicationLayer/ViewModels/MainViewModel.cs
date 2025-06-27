using BusinessLayer;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class MainViewModel
    {
        public TourList tourList = BusinessManager.GetTourList();

        private readonly AMDSelectionListViewModel tourAMDSelectionList;
        private readonly TourDisplayViewModel tourDisplay;

        public MainViewModel(AMDSelectionListViewModel tourAMDSelectionList, TourDisplayViewModel tourDisplay)
        {
            this.tourAMDSelectionList = tourAMDSelectionList;
            this.tourDisplay = tourDisplay;
        }

        private void ChangeTour(Tour tour)
        {
            tourList.ChangeTour(tour);
        }
    }
}
