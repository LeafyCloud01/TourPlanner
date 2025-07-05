using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class SelectionListViewModel : INotifyPropertyChanged
    {
        public static TourList _TourList = BusinessManager.GetTourListDb();
        public ObservableCollection<Tour> _Tours = _TourList.GetTours();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ShowTour {  get; set; }

        public ObservableCollection<Tour> Tours
        {
            get => _Tours;
            set
            {
                _Tours = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tours)));
            }
        }

        public SelectionListViewModel() 
        {
            CreateShowTour();

            Messenger.Default.Register<TourList>(this, (action) => ReceiveCurrentTourList(action)); 
        }

        private void ReceiveCurrentTourList(TourList CurrentTours)
        {
            Tours = CurrentTours.GetTours();
            Messenger.Default.Send<Tour>(CurrentTours.tours[0]);
        }

        private void CreateShowTour() { ShowTour = new RelayCommand<int>(ShowTourExecute); }
        public void ShowTourExecute(int Param)
        {
            Tour tourToSend = _TourList.getTour(Param);
            TourLog logToSend = (tourToSend.logs.logs.Count > 0) ? tourToSend.logs.logs[0] : new TourLog();

            Messenger.Default.Send<Tour>(tourToSend);
            Messenger.Default.Send<TourLog>(logToSend);
            Messenger.Default.Send<string>("default_tabs");
        }
    }
}
