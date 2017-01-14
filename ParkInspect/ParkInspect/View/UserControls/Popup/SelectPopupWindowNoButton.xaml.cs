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
using System.Windows.Shapes;
using MahApps.Metro.SimpleChildWindow;

namespace ParkInspect.View.UserControls.Popup
{
    /// <summary>
    /// Interaction logic for BaseChildWindow.xaml
    /// </summary>
    public partial class SelectPopupWindowNoButton : ChildWindow
    {
        /// <summary>
        /// Gets or sets additional content for the UserControl
        /// </summary>
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(SelectPopupWindowNoButton),
              new PropertyMetadata(null));

        public SelectPopupWindowNoButton()
        {
            InitializeComponent();
        }
    }
}
