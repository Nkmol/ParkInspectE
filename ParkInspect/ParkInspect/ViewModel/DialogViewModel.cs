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

        public void ShowMessage(string title, string message)
        {
            DialogCoordinator.ShowMessageAsync(this, title, message);
        }

    }
}
