using GalaSoft.MvvmLight;
using ParkInspect.View.UserControls.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkInspect.ViewModel
{
    public class PopupManager
    {
        private PopupCoordinator PopupCoordinator;

        public PopupManager(PopupCoordinator popupCoordinator)
        {
            PopupCoordinator = popupCoordinator;
        }

        // <summary>ShowPopup shows a MahApps Popup Selection window</summary>
        // <param name="title">Title for the popup.<param/>
        // <param name="content">Content (UserControl) that you want to show inside the popup.</param>
        // <param name="selectaction">Task of what should happen on select button click.</param>
        // <typeparam name="T" ></ typeparam >
        public Task ShowPopup<T>(string title, UserControl content, Action<T> selectaction) where T : class
        {
            return PopupCoordinator.ShowSelectPopupAsync<T>(this, title, content, selectaction);
        }

        public Task ShowConfirmPopup<T>(string title, UserControl content, Action<T> action) where T : class
        {
            return PopupCoordinator.ShowConfirmPopupAsync<T>(this, title, content, action);
        }
    }
}
