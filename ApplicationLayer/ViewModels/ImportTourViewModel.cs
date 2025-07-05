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

        public ImportTourViewModel()
        {
            CreateImportTour();
        }

        private void CreateImportTour() { ImportTour = new RelayCommand<string>(ImportTourExecute); }

        private void ImportTourExecute(string Format)
        {
            bool imported = BusinessManager.ImportTour(Format);
            Messenger.Default.Send<TourList>(BusinessManager.GetTourList());
            if(imported) { MessageBox.Show("Tour Successfully Imported!"); }
            else { MessageBox.Show("An error occurred importing tour from json"); }
        }
    }
}
