using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class TourDisplayViewModel
    {
        private readonly TourInfoDisplayViewModel tourInfoDisplay;
        private readonly RouteInfoDisplayViewModel routeInfoDisplay;
        private readonly TourInputsViewModel tourInputs;
        private readonly LogInfoDisplayViewModel logInfoDisplay;
        private readonly LogInputsViewModel logInputs;
        private readonly AMDControlsViewModel logAMDControls;
        private readonly DeleteTourViewModel deleteTour;

        public TourDisplayViewModel(TourInfoDisplayViewModel tourInfoDisplay, RouteInfoDisplayViewModel routeInfoDisplay, TourInputsViewModel tourInputs, LogInfoDisplayViewModel logInfoDisplay, LogInputsViewModel logInputs, AMDControlsViewModel logAMDControls, DeleteTourViewModel deleteTour)
        {
            this.tourInfoDisplay = tourInfoDisplay;
            this.routeInfoDisplay = routeInfoDisplay;
            this.tourInputs = tourInputs;
            this.logInfoDisplay = logInfoDisplay;
            this.logInputs = logInputs;
            this.logAMDControls = logAMDControls;
            this.deleteTour = deleteTour;
        }
    }
}
