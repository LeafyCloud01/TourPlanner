using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class RouteInfoDisplayViewModel
    {
        public string ImageSource = "/Views/PlaceholderMap.jpg";
        private Tour CurrentTour;
        public RouteInfoDisplayViewModel()
        {
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
            TourList currentTours = BusinessManager.GetTourList();
            CurrentTour = (currentTours.tours.Count > 0) ? currentTours.tours[0] : new Tour();
            ImageSource = CurrentTour.RouteInformation;
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTour = CurrentTour;
            ImageSource = CurrentTour.RouteInformation;
        }
    }
}
