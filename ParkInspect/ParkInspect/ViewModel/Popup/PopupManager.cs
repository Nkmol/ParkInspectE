using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using ParkInspect.View.UserControls.Popup;

namespace ParkInspect.ViewModel.Popup
{
    public class PopupManager
    {
        private readonly PopupCoordinator _popupCoordinator;

        public PopupManager(PopupCoordinator popupCoordinator)
        {
            _popupCoordinator = popupCoordinator;
        }

        // <summary>ShowPopup shows a MahApps Popup Selection window</summary>
        // <param name="title">Title for the popup.<param/>
        // <param name="content">Content (UserControl) that you want to show inside the popup.</param>
        // <param name="selectaction">Task of what should happen on select button click.</param>
        // <typeparam name="T" ></ typeparam >
        public Task ShowPopup<T>(string title, UserControl content, Action<T> selectaction) where T : ViewModelBase, IPopup
        {
            return _popupCoordinator.ShowSelectPopupAsync<T>(this, title, content, selectaction);
        }

        public Task ShowUpdateNewPopup<T>(string title, UserControl content, Action<T> action, Action<T> initAction = null, T dataContext = null) where T : ViewModelBase, ICreateUpdatePopup
        {
            return _popupCoordinator.ShowUpdateNewPopupAsync<T>(this, title, content, action, initAction, dataContext);
        }
    }
}
