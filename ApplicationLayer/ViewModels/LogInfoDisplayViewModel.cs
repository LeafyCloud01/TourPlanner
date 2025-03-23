using BusinessLayer;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class LogInfoDisplayViewModel : INotifyPropertyChanged
    {
        public static LogList _Logs = new LogList();
        public ObservableCollection<TourLog> Logs = _Logs.GetLogs();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<TourLog> LogInfo
        {
            get => Logs;
            set
            {
                Logs = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogInfo)));
            }
        }

        public LogInfoDisplayViewModel() { }
    }
}
