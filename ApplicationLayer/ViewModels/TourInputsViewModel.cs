using BusinessLayer;
using GalaSoft.MvvmLight.Command;
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
        public static Tour _TourInfo = new Tour();

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler<Tour> TourInputsChanged;

        public ICommand ChangeTour { get; internal set; }
        
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
        }

        private void CreateChangeTour() { ChangeTour = new RelayCommand(ChangeTourExecute); }
        public void ChangeTourExecute() 
        { 
            // add functionality
        }

        public void isEdit()
        {
            Name = _TourInfo.Name;
            Description = _TourInfo.Description;
            From = _TourInfo.From;
            To = _TourInfo.To;
            TransportType = _TourInfo.transportType;
        }
        
    }
}
