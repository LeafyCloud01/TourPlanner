using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class TourDisplayViewModel
    {
        public Tour tour = new Tour();

        private readonly TourInfoDisplayViewModel tourInfoDisplay;
        private readonly RouteInfoDisplayViewModel routeInfoDisplay;
        private readonly TourInputsViewModel tourInputs;
        private readonly LogInfoDisplayViewModel logInfoDisplay;
        private readonly LogInputsViewModel logInputs;
        private readonly AMDControlsViewModel logAMDControls;
        public TourDisplayViewModel(TourInfoDisplayViewModel tourInfoDisplay, RouteInfoDisplayViewModel routeInfoDisplay, TourInputsViewModel tourInputs, LogInfoDisplayViewModel logInfoDisplay, LogInputsViewModel logInputs, AMDControlsViewModel logAMDControls) 
        {
            tourInfoDisplay.TourInfo = tour;
        }
    }
}
