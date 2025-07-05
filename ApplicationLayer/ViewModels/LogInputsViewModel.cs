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
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class LogInputsViewModel
    {
        public TourLog _LogInfo;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Tour CurrentTour { get; set; }

        public ICommand ChangeLog {  get; set; }

        public TourLog LogInfo
        {
            get => _LogInfo;
            set
            {
                _LogInfo = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogInfo)));
            }
        }
        public LogInputsViewModel() 
        {
            TourList currentTours = BusinessManager.GetTourList();
            LogList currentLogs = (currentTours.tours.Count > 0) ? currentTours.tours[0].logs : new LogList(); 
            _LogInfo = (currentLogs.logs.Count > 0) ? currentTours.tours[0].logs.logs[0] : new TourLog();
            CurrentTour = (currentTours.tours.Count > 0) ? BusinessManager.GetTourList().tours[0] : new Tour();

            CreateChangeLog();

            Messenger.Default.Register<TourLog>(this, (action) => ReceiveCurrentTourLog(action));
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void ReceiveCurrentTour(Tour tour)
        {
            this.CurrentTour = tour;
        }
        private void ReceiveCurrentTourLog(TourLog CurrentLog)
        {
            this.LogInfo = CurrentLog;
        }

        private void CreateChangeLog() { ChangeLog = new RelayCommand(ChangeLogExecute); }
        public void ChangeLogExecute()
        {
            Tour currentTour = CurrentTour;
            BusinessManager.ChangeLog(CurrentTour.ID, _LogInfo);
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            Messenger.Default.Send<Tour>(currentTour);
            Messenger.Default.Send<string>("log_list");
        }
    }
}
