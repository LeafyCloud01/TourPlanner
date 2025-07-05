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
    public class ImportTourViewModel
    {
        public ICommand ImportTour { get; set; }

        public ImportTourViewModel()
        {
            CreateExportTour();
        }

        private void CreateExportTour() { ImportTour = new RelayCommand(ImportTourExecute); }

        private void ImportTourExecute()
        {
            bool imported = BusinessManager.ImportTour();
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            if(imported) { MessageBox.Show("Tour Successfully Imported!"); }
        }
    }
}
