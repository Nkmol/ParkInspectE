using GalaSoft.MvvmLight;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Domain;
using ParkInspect.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class DashboardViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        public int Height = 100;
        public int Width = 100;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        public ObservableCollection<Users> klanten;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public DashboardViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });

            klanten = new ObservableCollection<Users>();
        }

        public async void showLoginDialog(MainWindow window)
        {
            bool logged = false;
            while (!logged)
            {
                LoginDialogData result = await window.ShowLoginAsync("Authentication", "Enter your password", new LoginDialogSettings { ColorScheme = window.MetroDialogOptions.ColorScheme });
                if (result != null)
                {
                    int rs = login(result.Username, result.Password);

                    if (rs == 0)
                    {
                        MessageDialogResult messageResult = await window.ShowMessageAsync("Error", "Incorrect username/password");
                    }
                    else if (rs == 1)
                    {
                        logged = true;
                        MessageDialogResult messageResult = await window.ShowMessageAsync("Welkom: " + result.Username, "Have a nice day!");                      
                    }
                }
            }
        }

        public int login(string username, string password)
        {
            using (var context = new Entities())
            {
                List<Users> list = context.Users.ToList();
                foreach(Users u in list)
                {
                    if(u.Username.Equals(username) && u.Password.Equals(password))
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}