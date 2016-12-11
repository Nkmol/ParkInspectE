using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace ParkInspect.ViewModel
{
    public class DialogViewModel
    {
        public IDialogCoordinator DialogCoordinator;

        public DialogViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
        }

        public async void ShowMessage(string title, string message)
        {
           await DialogCoordinator.ShowMessageAsync(this, title, message);
        }

        public async Task<LoginDialogData> ShowLogin(string title, string message, LoginDialogSettings settings )
        {
          LoginDialogData result = await DialogCoordinator.ShowLoginAsync(this, title, message, settings);

          return result;
        }

    }
}
