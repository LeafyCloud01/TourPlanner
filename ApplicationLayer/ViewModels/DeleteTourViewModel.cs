using BusinessLayer;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class DeleteTourViewModel
    {
        public int CurrentTourID;
        public ICommand DeleteTour { get; set; }

        public DeleteTourViewModel()
        {
            TourList currentTours = BusinessManager.GetTourListDb();
            CurrentTourID = (currentTours.tours.Count > 0) ? currentTours.tours[0].ID : -1;

            CreateDeleteTour();
            Messenger.Default.Register<Tour>(this, (action) => ReceiveCurrentTour(action));
        }

        private void CreateDeleteTour() { DeleteTour = new RelayCommand(DeleteTourExecute); }

        private void DeleteTourExecute()
        {
            MessageBoxResult confirmDelete = MessageBox.Show("Confirm Deleting Tour", "Confirm Delete", MessageBoxButton.YesNo);
            switch (confirmDelete)
            {
                case MessageBoxResult.Yes:
                    BusinessManager.DeleteTour(CurrentTourID);
                    Messenger.Default.Send<TourList>(BusinessManager.GetTourListDb());
                    Messenger.Default.Send<string>("default_tabs");
                break;
            }
        }
        private void ReceiveCurrentTour(Tour CurrentTour)
        {
            this.CurrentTourID = CurrentTour.ID;
        }
    }
}
