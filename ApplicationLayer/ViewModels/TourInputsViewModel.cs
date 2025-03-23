using BusinessLayer;
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
    public class TourInputsViewModel
    {
        public Tour TourInfo = new Tour();

        /*public Tour _TourInfo = new Tour();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Tour TourInfo
        {
            get => _TourInfo;
            set
            {
                _TourInfo = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TourInfo)));
            }
        }*/

        public event EventHandler<Tour> TourInputsChanged;

        public ICommand ChangeTour {  get; }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; }
        }

        private string _from;
        public string From
        {
            get => _from;
            set { _from = value; }
        }

        private string _to;
        public string To
        {
            get => _to;
            set { _to = value; }
        }

        private string _transportType;
        public string TransportType
        {
            get => _transportType;
            set { _transportType = value; }
        }

        public TourInputsViewModel()
        {
            /*
            ChangeTour = new ChangeTour((_) => 
            { 
                this.TourInputsChanged?.Invoke(this, Name);
            });
            */
        }

        public void isEdit(Tour tour)
        {
            Name = tour.Name;
            Description = tour.Description;
            From = tour.From;
            To = tour.To;
            TransportType = tour.TransportType;
        }
    }

    public class ChangeTour
    {
        public ChangeTour()
        {

        }
    }
}
