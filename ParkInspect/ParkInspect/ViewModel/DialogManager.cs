using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class DialogManager
    {
        protected EmployeeService _service;

        public DialogManager(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
        }

        // Virtual = Used for Moq
        public virtual IDialogCoordinator DialogCoordinator { get; set; }

        public EmployeeService Service
        {
            set { _service = value; }
        }

        public void ShowMessage(string title, string message)
        {
            DialogCoordinator.ShowMessageAsync(this, title, message);
        }

        public async Task<LoginDialogData> ShowLogin(string title, string message, LoginDialogSettings settings)
        {
            return await DialogCoordinator.ShowLoginAsync(this, title, message, settings);
        }

        public async void ShowLoginDialog(LoginViewModel lv, LoginDialogSettings LogindialogSetting = null)
        {
            var loginDialogSettings = LogindialogSetting ?? new LoginDialogSettings
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
                    var sha = SHA256.Create();

                    var bytes = new byte[result.Password.Length*sizeof(char)];
                    Buffer.BlockCopy(result.Password.ToCharArray(), 0, bytes, 0, bytes.Length);

                    sha.ComputeHash(bytes);

                    var chars = new char[sha.Hash.Length/sizeof(char)];
                    Buffer.BlockCopy(sha.Hash, 0, chars, 0, sha.Hash.Length);

                    var rs = _service.Login(result.Username, result.Password);

                    if (!rs)
                    {
                        if (result.ShouldRemember)
                            loginDialogSettings.InitialUsername = result.Username;

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
                        lv.CurrentUser = lv.Service.GetEmployee(result.Username, result.Password);
                        lv.Dashboard.ChangeAuthorization(lv.CurrentUser.Role1);
                    }
                }
            }
        }
    }
}