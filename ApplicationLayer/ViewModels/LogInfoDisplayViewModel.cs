using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class LogInfoDisplayViewModel : INotifyPropertyChanged
    {
        public static LogList _Logs = BusinessManager.GetTourList().tours[0].logs;
        public ObservableCollection<TourLog> Logs = _Logs.GetLogs();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand AddLog {  get; set; }
        public ICommand EditLog { get; set; }
        public ICommand DeleteLog { get; set; }
        public Tour CurrentTour { get; set; }

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

        public LogInfoDisplayViewModel() 
        {
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
            CreateAddLog();
            CreateEditLog();
            CreateDeleteLog();
            CurrentTour = BusinessManager.GetTourList().tours[0];
        }

        private void CreateAddLog() { AddLog = new RelayCommand<string>(AddLogExecute); }
        private void CreateEditLog() { EditLog = new RelayCommand<string>(EditLogExecute); }
        private void CreateDeleteLog() { DeleteLog = new RelayCommand<int>(DeleteLogExecute); }

        private void ReceiveCurrentTour(Tour NewTour)
        {
            CurrentTour = NewTour;
            this.LogInfo = CurrentTour.logs.GetLogs();
        }
        private void AddLogExecute(string log)
        {
            Messenger.Default.Send<string>("add_log");
            Messenger.Default.Send<TourLog>(new TourLog());
        }
        private void EditLogExecute(string log)
        {
            Messenger.Default.Send<string>("edit_log");
        }
        private void DeleteLogExecute(int log)
        {
            MessageBoxResult confirmDelete = MessageBox.Show("Confirm Deleting Log", "Confirm Delete", MessageBoxButton.YesNo);
            switch (confirmDelete)
            {
                case MessageBoxResult.Yes:
                    Tour currentTour = CurrentTour;
                    BusinessManager.DeleteLog(CurrentTour.ID, log);
                    Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
                    Messenger.Default.Send<Tour>(currentTour);
                    break;
            }
        }
    }
}
