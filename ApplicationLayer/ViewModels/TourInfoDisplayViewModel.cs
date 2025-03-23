using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class TourInfoDisplayViewModel : INotifyPropertyChanged
    {
        public Tour _TourInfo = new Tour();

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
        }
        public TourInfoDisplayViewModel()
        {
            
        }
    }
}
