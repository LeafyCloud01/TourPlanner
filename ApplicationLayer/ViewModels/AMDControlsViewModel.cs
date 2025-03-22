using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PresentationLayer.ViewModels
{
    public class AMDControlsViewModel
    {
        public ICommand AddElement { get; } //calls add X method in superceding view model
        public ICommand DeleteElement { get; } //calls delete X method in superceding view model
        public ICommand ModifyElement { get; } //calls modify X method in superceding view model

        public AMDControlsViewModel()
        {
            //Implement
        }
    }
}
