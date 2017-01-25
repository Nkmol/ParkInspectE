using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        private bool _loginButtonEnabled;

        private string _loginName;

        private bool _logoutButtonEnabled;

        public Employee CurrentUser;

        private ICommand _showLoginDialogCommand;

        private ICommand _logoutCommand;

        /// <summary>
        ///     Initializes a new instance of the LoginViewModel class.
        /// </summary>


        private readonly DialogManager _dialogViewModel;

        public DashboardViewModel Dashboard;



        public EmployeeService Service;

        public LoginViewModel(IRepository context, DashboardViewModel dashboard, DialogManager dialogViewModel)
        {
            _dialogViewModel = dialogViewModel;
            _dialogViewModel.Service = new EmployeeService(context);

            Service = new EmployeeService(context);
            LoginButtonEnabled = true;
            LogoutButtonEnabled = false;

            Dashboard = dashboard;



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
                                                  ?? (_showLoginDialogCommand = new RelayCommand(() => _dialogViewModel.ShowLoginDialog(this)));

        public ICommand LogoutCommand => _logoutCommand
                                                  ?? (_logoutCommand = new RelayCommand(Logout));
       
        private void Logout()
        {
            LoginName = "";
            LoginButtonEnabled = true;
            LogoutButtonEnabled = false;
            Dashboard.ChangeAuthorization(null);
            _dialogViewModel.ShowLoginDialog(this);
        }
    }
}