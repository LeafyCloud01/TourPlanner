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

        public string Comment
        {
            get => _LogInfo.comment;
            set { _LogInfo.comment = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment))); }
        }
        public string Difficulty
        {
            get => _LogInfo.difficulty.ToString();
            set { _LogInfo.difficulty = float.Parse(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Difficulty))); }
        }
        public string TotalDistance
        {
            get => _LogInfo.totalDistance.ToString();
            set { _LogInfo.totalDistance = float.Parse(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalDistance))); }
        }
        public string TotalTime
        {
            get => _LogInfo.totalTime.ToShortTimeString();
            set { _LogInfo.totalTime = TimeOnly.Parse(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalTime))); }
        }
        public string Rating
        {
            get => _LogInfo.rating.ToString();
            set { _LogInfo.rating = float.Parse(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rating))); }
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
            this.Comment = CurrentLog.comment;
            this.Difficulty = CurrentLog.difficulty.ToString();
            this.TotalDistance = CurrentLog.totalDistance.ToString();
            this.TotalTime = CurrentLog.totalTime.ToShortTimeString();
            this.Rating = CurrentLog.rating.ToString();
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
