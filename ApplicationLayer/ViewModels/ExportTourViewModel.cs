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

        public ExportTourViewModel()
        {
            CurrentTourID = BusinessManager.GetTourList().tours[0].ID;
            CreateExportTour();
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void CreateExportTour() { ExportTour = new RelayCommand<string>(ExportTourExecute); }

        private void ExportTourExecute(string Format)
        {
            bool exported = BusinessManager.ExportTour(CurrentTourID, Format);
            if(exported) { MessageBox.Show("Tour Successfully Exported!"); }
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTourID = CurrentTour.ID;
        }
    }
}
