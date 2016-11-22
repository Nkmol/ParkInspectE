using System.Linq;
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

        private ICommand _showLoginDialogCommand;

        /// <summary>
        ///     Initializes a new instance of the LoginViewModel class.
        /// </summary>
        protected EmployeeService Service;

        public LoginViewModel(IDialogCoordinator dialogCoordinator, IRepository context)
        {
            _dialogCoordinator = dialogCoordinator;
            Service = new EmployeeService(context);
            LoginName = "Login";
            LoginButtonEnabled = true;
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

        public ICommand ShowLoginDialogCommand
        {
            get
            {
                return _showLoginDialogCommand
                       ?? (_showLoginDialogCommand = new RelayCommand(ShowLoginDialog));
            }
        }

        public async void ShowLoginDialog()
        {
            var loginDialogSettings = new LoginDialogSettings
            {
                UsernameWatermark = "Emailadres...",
                PasswordWatermark = "Wachtwoord...",
                NegativeButtonVisibility = Visibility.Visible
            };

            var result = await _dialogCoordinator.ShowLoginAsync(this, "Authenticatie", "Voer uw inloggegevens in", loginDialogSettings);

            if (result == null)
                return;

            var rs = Service.GetEmployee(result.Username, result.Password).Count() != 0;

            if (!rs)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Oeps er is iets misgegaan", "Ongeldig email/wachtwoord");
            }
            else
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Welkom: " + result.Username, "Fijne dag!");
                LoginName = result.Username;
                LoginButtonEnabled = false;
            }
        }
    }
}