using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer.ViewModels
{
    public class SearchBoxViewModel : INotifyPropertyChanged
    {
        public string _searchText = "";
        public event PropertyChangedEventHandler? PropertyChanged;

        public string searchText
        {
            get => _searchText;
            set
            {
                _searchText = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(searchText)));

                updateSearch(_searchText);
            }
        }

        private void updateSearch(string text)
        {
            if(text != "") { Messenger.Default.Send<TourList>(BusinessManager.GetTourList(text)); }
            else { Messenger.Default.Send<TourList>(BusinessManager.GetTourList()); }
        }

        public SearchBoxViewModel() { }
    }
}
