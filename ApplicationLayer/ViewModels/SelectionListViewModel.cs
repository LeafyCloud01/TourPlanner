using BusinessLayer;
using GalaSoft.MvvmLight.Command;
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
        public static TourList _TourList = new TourList();
        public ObservableCollection<Tour> _Tours = _TourList.GetTours();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ShowTour {  get; internal set; }

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
        }

        private void CreateShowTour() { ShowTour = new RelayCommand<string>(ShowTourExecute); }
        public void ShowTourExecute(string Param)
        {
            MessageBox.Show(Param);
        }
    }
}
