using BusinessLayer;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.Views
{
    /// <summary>
    /// Interaction logic for TourDisplay.xaml
    /// </summary>
    public partial class TourDisplay : UserControl
    {
        public TourDisplay()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(this, (action) => ReceiveDesiredView(action));
        }

        private void ReceiveDesiredView(string view)
        {
            showTab(view);
        }

        private Dictionary<string, List<string>> tabGroups = new Dictionary<string, List<string>>
        {
            // First tab header in list is the one that will be focused on
            {"add_tour", ["Add Tour"] },
            {"add_log", ["Add Log", "Tour Info", "Route Info", "Tour Logs", "Edit Tour", "Delete Tour"] },
            {"edit_log", ["Edit Log", "Tour Info", "Route Info", "Tour Logs", "Edit Tour", "Delete Tour"] },
            {"delete_log", ["Delete Log", "Tour Info", "Route Info", "Tour Logs", "Edit Tour", "Delete Tour"] },
            {"log_list", ["Tour Logs", "Tour Info", "Route Info", "Edit Tour", "Delete Tour"] },
            {"default_tabs", ["Tour Info", "Route Info", "Tour Logs", "Edit Tour", "Delete Tour"] }
        };

        private void hideAllTabs()
        {
            foreach (var item in TourDisplayTabControl.Items)
            {
                if (item is TabItem tabItem)
                {
                    tabItem.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void showTab(string tab)
        {
            hideAllTabs();
            foreach(var item in TourDisplayTabControl.Items)
            {
                if(item is TabItem tabItem){
                    if (tabGroups[tab].Contains(tabItem.Header.ToString()))
                    {
                        tabItem.Visibility = Visibility.Visible;
                        tabItem.IsSelected = (tabItem.Header.ToString() == tabGroups[tab][0]) ? true : false;
                    }
                    
                }
            }
        }
    }
}
