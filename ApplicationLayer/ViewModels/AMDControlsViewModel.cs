using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class AMDControlsViewModel
    {
        public ICommand AddElement { get; internal set; } //calls add X method in superceding view model
        public ICommand DeleteElement { get; internal set; } //calls delete X method in superceding view model
        public ICommand ModifyElement { get; internal set; } //calls modify X method in superceding view model

        public AMDControlsViewModel()
        {
            CreateAddElement();
            CreateDeleteElement();
            CreateModifyElement();
        }

        private void CreateAddElement() { AddElement = new RelayCommand(AddElementExecute); }
        private void CreateDeleteElement() { DeleteElement = new RelayCommand(DeleteElementExecute); }
        private void CreateModifyElement() { ModifyElement = new RelayCommand(ModifyElementExecute); }

        public void AddElementExecute()
        {
            MessageBox.Show("Add");
        }
        public void DeleteElementExecute()
        {
            MessageBox.Show("Delete");
        }
        public void ModifyElementExecute()
        {
            MessageBox.Show("Modify");
        }
    }
}
