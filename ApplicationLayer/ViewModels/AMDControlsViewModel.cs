using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;

namespace PresentationLayer.ViewModels
{
    public class AMDControlsViewModel
    {
        public ICommand AddElement { get; set; }
        public ICommand DeleteElement { get; set; }
        public ICommand ModifyElement { get; set; }

        public AMDControlsViewModel()
        {
            CreateAddElement();
            CreateDeleteElement();
            CreateModifyElement();
        }

        private void CreateAddElement() { AddElement = new RelayCommand<string>(AddElementExecute); }
        private void CreateDeleteElement() { DeleteElement = new RelayCommand<string>(DeleteElementExecute); }
        private void CreateModifyElement() { ModifyElement = new RelayCommand<string>(ModifyElementExecute); }

        private void AddElementExecute(string type)
        {
            Messenger.Default.Send<string>("add_" + type);
            Messenger.Default.Send<Tour>(new Tour());
        }
        private void DeleteElementExecute(string type)
        {
            Messenger.Default.Send<string>("delete_" + type);
        }
        private void ModifyElementExecute(string type)
        {
            Messenger.Default.Send<string>("edit_" + type);
        }
    }
}
