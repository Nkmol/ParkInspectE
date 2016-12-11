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

         // <summary>Shows a MahApps Selection window
         // <para>title - Title for the popup.<para/>
         // <para> content - Content (UserControl) that you want to show inside the popup.</para>
         // <para> selectaction - Task of what should happen on select button click.</para>
         // <typeparam name = "T" ></ typeparam >
         // </summary>
        public Task ShowPopup<T>(string title, UserControl content, Action<T> selectaction) where T : class
        {
            return PopupCoordinator.ShowSelectPopupAsync<T>(this, title, content, selectaction);
        }
    }
}
