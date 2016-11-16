using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParkInspect.Model
{
    /*
     * Class that manages all the confirmation dialogs
     * 
     */
    public class WindowManager
    {
        public static MessageBoxResult Confirm()
        {
            MessageBoxResult messageBoxResult =
                System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

            return messageBoxResult;
        }

        public static void ShowMessage(string Message)
        {
            MessageBox.Show("" + Message);
        }
    }
}
