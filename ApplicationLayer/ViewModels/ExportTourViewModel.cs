using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using PresentationLayer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class ExportTourViewModel
    {
        public int CurrentTourID;
        public ICommand ExportTour { get; set; }
        public ICommand ExportTours { get; set; }

        public ExportTourViewModel()
        {
            TourList currentTours = BusinessManager.GetTourList();
            CurrentTourID = (currentTours.tours.Count > 0) ? currentTours.tours[0].ID : -1;

            CreateExportTour();
            CreateExportTours();
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void CreateExportTour() { ExportTour = new RelayCommand<string>(ExportTourExecute); }
        private void CreateExportTours() { ExportTours = new RelayCommand<string>(ExportToursExecute); }

        private void ExportTourExecute(string Format)
        {
            bool exported = BusinessManager.ExportTour(CurrentTourID, Format);
            if(exported == true) { MessageBox.Show("Tour Successfully Exported!"); }
            else if(exported == false) { MessageBox.Show($"An error occurred exporting tour as {Format}"); }
        }
        private void ExportToursExecute(string Format)
        {
            bool exported = BusinessManager.ExportTours(Format);
            if (exported == true) { MessageBox.Show("Tour List Successfully Exported!"); }
            else if (exported == false) { MessageBox.Show($"An error occurred exporting tour as {Format}"); }
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTourID = CurrentTour.ID;
        }
    }
}
