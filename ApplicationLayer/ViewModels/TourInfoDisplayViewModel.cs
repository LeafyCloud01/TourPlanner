using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer.ViewModels
{
    public class TourInfoDisplayViewModel : INotifyPropertyChanged
    {
        public Tour _TourInfo;

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
            _TourInfo = BusinessManager.GetTourList().tours[0];

            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.TourInfo = CurrentTour;
        }
    }
}