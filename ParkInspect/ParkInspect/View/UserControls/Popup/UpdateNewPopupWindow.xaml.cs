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
    /// Interaction logic for ConfirmPopupWIndow.xaml
    /// </summary>
    public partial class UpdateNewPopupWindow : ChildWindow
    {

        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(UpdateNewPopupWindow),
              new PropertyMetadata(null));

        public UpdateNewPopupWindow()
        {
            InitializeComponent();
        }
    }
}
