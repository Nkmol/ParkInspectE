using System.Windows;
using MahApps.Metro.Controls;
using ParkInspect.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using System.Collections;

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

        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private async void ShowLoginDialog(object sender, RoutedEventArgs e)
        {
            var vm = (LoginViewModel)Resources["LoginViewModel"];

            bool logged = false;

            while (!logged)
            {
                LoginDialogData result = await this.ShowLoginAsync("Authentication", "Enter your credentials", new LoginDialogSettings { ColorScheme = this.MetroDialogOptions.ColorScheme });
                if (result != null)
                {
                    int rs = vm.login(result.Username, result.Password);

                    if (rs == 0)
                    {
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Error", "Incorrect username/password");
                    }
                    else if (rs == 1)
                    {
                        logged = true;
                        MessageDialogResult messageResult = await this.ShowMessageAsync("Welcome: " + result.Username, "Have a nice day!");
                    }
                }
            }
        }
    }
}