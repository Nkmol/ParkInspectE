using System.Windows.Media.Converters;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;

namespace ParkInspect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// 

       


        private DashboardViewModel _datacontext;
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            if (DataContext.GetType() == typeof(DashboardViewModel)) _datacontext = (DashboardViewModel)DataContext;


        }
    }
}
