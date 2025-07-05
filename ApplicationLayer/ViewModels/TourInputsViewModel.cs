using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace PresentationLayer.ViewModels
{
    public class TourInputsViewModel : INotifyPropertyChanged
    {
        public static Tour _TourInfo = BusinessManager.GetTourList().tours[0];

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<Tour> TourInputsChanged;

        public ICommand ChangeTour { get; set; }
        
        public string Name
        {
            get => _TourInfo.name;
            set { _TourInfo.name = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); }
        }

        public string Description
        {
            get => _TourInfo.description;
            set { _TourInfo.description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description))); }
        }

        public string From
        {
            get => _TourInfo.from;
            set { _TourInfo.from = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(From))); }
        }

        public string To
        {
            get => _TourInfo.to;
            set { _TourInfo.to = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(To))); }
        }

        public Transport TransportType
        {
            get => _TourInfo.transportType;
            set { _TourInfo.transportType = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TransportType))); }
        }
        
        public TourInputsViewModel()
        {
            CreateChangeTour();

            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.Name = CurrentTour.name;
            this.Description = CurrentTour.description;
            this.From = CurrentTour.from;
            this.To = CurrentTour.to;
            this.TransportType = CurrentTour.transportType;
        }

        private void CreateChangeTour() { ChangeTour = new RelayCommand(ChangeTourExecute); }
        public void ChangeTourExecute() 
        {
            BusinessManager.ChangeTour(_TourInfo);
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            Messenger.Default.Send<string>("default_tabs");
        }
    }
}
