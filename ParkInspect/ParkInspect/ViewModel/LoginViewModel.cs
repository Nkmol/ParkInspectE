using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

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

        private readonly IDialogCoordinator _dialogCoordinator;

        private ICommand _showLoginDialogCommand;

        public ICommand ShowLoginDialogCommand
        {
            get
            {
                return _showLoginDialogCommand
                       ?? (_showLoginDialogCommand = new RelayCommand(showLoginDialog));
            }
        }

        public LoginViewModel(IDialogCoordinator dialogCoordinator, IRepository context)
        {
            this._dialogCoordinator = dialogCoordinator;
            Service = new EmployeeService(context);
        }

        public async void showLoginDialog()
        {
            bool logged = false;

            while (!logged)
            {
                LoginDialogData result = await _dialogCoordinator.ShowLoginAsync(this, "Authentication", "Enter your credentials");
                if (result != null)
                {
                    bool rs = login(result.Username, result.Password);

                    if (rs)
                    {
                        logged = true;
                        MessageDialogResult messageResult = await _dialogCoordinator.ShowMessageAsync(this, "Welcome: " + result.Username, "Have a nice day!");                      
                    }
                    else
                    {
                        MessageDialogResult messageResult = await _dialogCoordinator.ShowMessageAsync(this, "Error", "Incorrect username/password");
                    }
                }
            }
        }

        public bool login(string email, string password)
        {
            IEnumerable<Employee> e = Service.GetEmployee(email, password);
            if (e.Count() != 0)
            {
                return true;
            }
            return false;
        }
    }
}