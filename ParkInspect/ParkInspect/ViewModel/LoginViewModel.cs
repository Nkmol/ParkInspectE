﻿using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        private bool _loginButtonEnabled;

        private string _loginName;

        private bool _logoutButtonEnabled;

        private Employee CurrentUser;

        private ICommand _showLoginDialogCommand;

        private ICommand _logoutCommand;

        /// <summary>
        ///     Initializes a new instance of the LoginViewModel class.
        /// </summary>
        protected EmployeeService Service;

        private DashboardViewModel dashboard;

        public LoginViewModel(IDialogCoordinator dialogCoordinator, IRepository context, DashboardViewModel dashboard)
        {
            _dialogCoordinator = dialogCoordinator;
            Service = new EmployeeService(context);
            LoginButtonEnabled = true;
            LogoutButtonEnabled = false;

            this.dashboard = dashboard;
        }

        public string LoginName
        {
            get { return _loginName; }
            set { Set(ref _loginName, value); }
        }

        public bool LoginButtonEnabled
        {
            get { return _loginButtonEnabled; }
            set { Set(ref _loginButtonEnabled, value); }
        }

        public bool LogoutButtonEnabled
        {
            get { return _logoutButtonEnabled; }
            set { Set(ref _logoutButtonEnabled, value); }
        }

        public ICommand ShowLoginDialogCommand => _showLoginDialogCommand
                                                  ?? (_showLoginDialogCommand = new RelayCommand(ShowLoginDialog));

        public ICommand LogoutCommand => _logoutCommand
                                                  ?? (_logoutCommand = new RelayCommand(Logout));

        public async void ShowLoginDialog()
        {
            var loginDialogSettings = new LoginDialogSettings
            {
                UsernameWatermark = "Emailadres...",
                PasswordWatermark = "Wachtwoord...",
                NegativeButtonVisibility = Visibility.Visible,
                RememberCheckBoxVisibility = Visibility.Visible
            };

            var logged = false;

            while (!logged)
            {
                var result =
                    await
                        _dialogCoordinator.ShowLoginAsync(this, "Authenticatie", "Voer uw inloggegevens in",
                            loginDialogSettings);

                if (result == null)
                    return;

                var rs = Service.GetEmployee(result.Username, result.Password).Count() != 0;

                if (!rs)
                {
                    if (result.ShouldRemember)
                    {
                        loginDialogSettings.InitialUsername = result.Username;
                    }

                    await
                        _dialogCoordinator.ShowMessageAsync(this, "Oeps er is iets misgegaan",
                            "Ongeldig email/wachtwoord");

                }
                else
                {
                    await _dialogCoordinator.ShowMessageAsync(this, "Welkom: " + result.Username, "Fijne dag!");
                    logged = true;
                    LoginName = result.Username;
                    LoginButtonEnabled = false;
                    LogoutButtonEnabled = true;

                    // goes wrong on multiple users with the same username and password with different roles.
                    CurrentUser = Service.GetEmployee(result.Username, result.Password).First();
                    dashboard.ChangeAuthorization(CurrentUser.Role1);

                }
            }
        }

        private void Logout()
        {
            LoginName = "";
            LoginButtonEnabled = true;
            LogoutButtonEnabled = false;
            dashboard.ChangeAuthorization(null);
        }
    }
}