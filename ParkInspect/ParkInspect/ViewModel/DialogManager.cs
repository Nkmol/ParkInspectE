using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class DialogManager
    {
        public IDialogCoordinator DialogCoordinator;

        protected EmployeeService _service;

        public EmployeeService Service
        {
            set { _service = value; }
        }

        public DialogManager(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
        }

        public void ShowMessage(string title, string message)
        {
            DialogCoordinator.ShowMessageAsync(this, title, message);
        }

        public async Task<LoginDialogData> ShowLogin(string title, string message, LoginDialogSettings settings)
        {
            LoginDialogData result = await DialogCoordinator.ShowLoginAsync(this, title, message, settings);

            return result;
        }

        public async void ShowLoginDialog(LoginViewModel lv)
        {
            var loginDialogSettings = new LoginDialogSettings
            {
                UsernameWatermark = "Emailadres...",
                PasswordWatermark = "Wachtwoord...",
                NegativeButtonVisibility = Visibility.Hidden,
                RememberCheckBoxVisibility = Visibility.Visible
            };

            var logged = false;

            while (!logged)
            {
                var result =
                    await
                        ShowLogin("Authenticatie", "Voer uw inloggegevens in",
                            loginDialogSettings);

                if (result != null)
                {

                    var rs = _service.GetEmployee(result.Username, result.Password).Count() != 0;

                    if (!rs)
                    {
                        if (result.ShouldRemember)
                        {
                            loginDialogSettings.InitialUsername = result.Username;
                        }


                        await DialogCoordinator.ShowMessageAsync(this, "Oeps er is iets misgegaan",
                            "Ongeldig email/wachtwoord");

                    }
                    else
                    {
                        await DialogCoordinator.ShowMessageAsync(this, "Welkom: " + result.Username, "Fijne dag!");
                        logged = true;

                        lv.LoginName = result.Username;
                        lv.LoginButtonEnabled = false;
                        lv.LogoutButtonEnabled = true;

                        // goes wrong on multiple users with the same username and password with different roles.
                        lv.CurrentUser = lv.Service.GetEmployee(result.Username, result.Password).First();
                        lv.Dashboard.ChangeAuthorization(lv.CurrentUser.Role1);
                    }
                }
            }
        }




    }
}
