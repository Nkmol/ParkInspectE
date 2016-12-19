using System;
using System.Collections.Generic;
using System.Linq;
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
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using ParkInspect.View.UserControls.Popup;
using ParkInspect.ViewModel;

namespace ParkInspect.View.UserControls
{
    /// <summary>
    /// Interaction logic for ParkeerplaatsControl.xaml
    /// </summary>
    public partial class ParkeerplaatsControl : UserControl
    {
        public ParkeerplaatsControl()
        {
            InitializeComponent();
        }

        public void Update(object sender, MouseButtonEventArgs e)
        {

            foreach (TabItem ti in Tabs.GetChildObjects())
            {
                if (ti.Header.Equals("Management"))
                {
                    Dispatcher.BeginInvoke((Action)(() => Tabs.SelectedItem = ti));
                    return;
                }
            }

        }

        public void Unselect(object sender, MouseButtonEventArgs e)
        {
            DataGrid.UnselectAll();
        }
    }
}
