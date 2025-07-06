using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
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
        public ICommand ImportTours { get; set; }

        public ImportTourViewModel()
        {
            CreateImportTour();
            CreateImportTours();
        }

        private void CreateImportTour() { ImportTour = new RelayCommand<string>(ImportTourExecute); }
        private void CreateImportTours() { ImportTours = new RelayCommand<string>(ImportToursExecute); }

        private void ImportTourExecute(string Format)
        {
            bool imported = BusinessManager.ImportTour(Format);
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            if(imported) { MessageBox.Show("Tour Successfully Imported!"); }
            else { MessageBox.Show($"An error occurred importing Tour from {Format}"); }
        }
        private void ImportToursExecute(string Format)
        {
            bool imported = BusinessManager.ImportTours(Format);
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            if (imported) { MessageBox.Show("Tour List Successfully Imported!"); }
            else { MessageBox.Show($"An error occurred importing Tour List from {Format}"); }
        }
    }
}
