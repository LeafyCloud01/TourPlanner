using BusinessLayer;
using GalaSoft.MvvmLight.Command;
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
        public TourLog _LogInfo = new TourLog();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ChangeLog {  get; internal set; }

        public TourLog LogInfo
        {
            get => _LogInfo;
            set
            {
                _LogInfo = value;

                // Support TwoWay binding
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogInfo)));
            }
        }
        public LogInputsViewModel() 
        {
            CreateChangeLog();
        }

        private void CreateChangeLog() { ChangeLog = new RelayCommand(ChangeLogExecute); }
        public void ChangeLogExecute()
        {

        }
    }
}
