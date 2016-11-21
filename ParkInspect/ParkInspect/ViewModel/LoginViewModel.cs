using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.Generic;
using System.Linq;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        /// 
        protected EmployeeService Service;

        public LoginViewModel(IRepository context)
        {
            Service = new EmployeeService(context);
        }

        public async void showLoginDialog(MainWindow window)
        {
            bool logged = false;

            while (!logged)
            {
                LoginDialogData result = await window.ShowLoginAsync("Authentication", "Enter your credentials", new LoginDialogSettings { ColorScheme = window.MetroDialogOptions.ColorScheme });
                if (result != null)
                {
                    bool rs = login(result.Username, result.Password);

                    if (rs)
                    {
                        logged = true;
                        MessageDialogResult messageResult = await window.ShowMessageAsync("Welcome: " + result.Username, "Have a nice day!");                      
                    }
                    else
                    {
                        MessageDialogResult messageResult = await window.ShowMessageAsync("Error", "Incorrect username/password");
                    }
                }
            }
        }

        public bool login(string email, string password)
        {
            if(Service.GetEmployee(email, password) != null)
            {
                return true;
            }
            return false;
        }
    }
}