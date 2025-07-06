using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace PresentationLayer.ViewModels
{
    public class RouteInfoDisplayViewModel
    {
        public System.Windows.Media.ImageSource ImageSource;
        private Tour CurrentTour;
        public RouteInfoDisplayViewModel()
        {
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
            TourList currentTours = BusinessManager.GetTourList();
            CurrentTour = (currentTours.tours.Count > 0) ? currentTours.tours[0] : new Tour();
            ImageSource = new BitmapImage(new Uri("https://64.media.tumblr.com/e71d53b39d804a21567f91184844b6f0/a045f54f93767126-37/s540x810/dedef96d2d8afceba7a3972a77384d1a248fd83e.pnj", UriKind.Absolute));
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTour = CurrentTour;
            ImageSource = new BitmapImage(new Uri("https://64.media.tumblr.com/e71d53b39d804a21567f91184844b6f0/a045f54f93767126-37/s540x810/dedef96d2d8afceba7a3972a77384d1a248fd83e.pnj",UriKind.Absolute));
        }
    }
}
