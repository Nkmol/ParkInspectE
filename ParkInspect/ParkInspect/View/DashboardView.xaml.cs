using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;

namespace ParkInspect.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        /// 




       
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            ButtonAutomationPeer peer = new ButtonAutomationPeer(LoginButton);

            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

          //  invokeProv.Invoke();
        }
    }
}
