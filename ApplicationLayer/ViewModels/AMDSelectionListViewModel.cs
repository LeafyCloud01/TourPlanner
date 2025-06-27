using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ViewModels
{
    public class AMDSelectionListViewModel
    {
        private readonly SearchBoxViewModel tourSearchBox;
        private readonly AMDControlsViewModel tourAMDControls;
        private readonly SelectionListViewModel tourSelectionList;

        public AMDSelectionListViewModel(SearchBoxViewModel tourSearchBox, AMDControlsViewModel tourAMDControls, SelectionListViewModel tourSelectionList)
        {
            this.tourSearchBox = tourSearchBox;
            this.tourAMDControls = tourAMDControls;
            this.tourSelectionList = tourSelectionList;
        }
    }
}
