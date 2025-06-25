using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class SearchBoxViewModel
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
            }
        }
        public SearchBoxViewModel() { }
    }
}
