using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class GenerateReportViewModel
    {
        public int CurrentTourID;
        public ICommand GenerateReport { get; set; }

        public GenerateReportViewModel()
        {
            TourList currentTours = BusinessManager.GetTourListDb();
            CurrentTourID = (currentTours.tours.Count > 0) ? currentTours.tours[0].ID : -1;

            CreateGenerateReport();
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void CreateGenerateReport() { GenerateReport = new RelayCommand<string>(GenerateReportExecute); }

        private void GenerateReportExecute(string Type)
        {
            bool report_created = BusinessManager.GenerateReport(CurrentTourID, Type);
            if (report_created) { MessageBox.Show("Report Successfully Created!"); }
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTourID = CurrentTour.ID;
        }
    }
}
