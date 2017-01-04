using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using ParkInspect.ViewModel;

namespace ParkInspect.View.UserControls.Popup
{
    public class PopupCoordinator
    {
        /// <summary>
        /// Gets the default instance if the popup coordinator, which can be injected into a view model.
        /// </summary>
        public static readonly PopupCoordinator Instance = new PopupCoordinator();

        /*
         * Shows a MahApps Selection window
         * @param context - The *registrated* ViewModel.
         * @param title - Title for the popup.
         * @param content - Content (UserControl) that you want to show inside the popup.
         * @param selectaction - Task of what should happen on select button click.
         */
        public Task ShowSelectPopupAsync<T>(object context, string title, UserControl content, Action<T> selectaction)
        {
            var window = GetMetroWindow(context);

            // Create BaseChildWindow
            var popupWindow = new SelectPopupWindow() {IsModal = true, AllowMove = true, AdditionalContent = content};

            // Fill Context with values so ViewModels are linked  // TODO Improve, let Injection handle more
            var Context = (PopupViewModel)popupWindow.DataContext;
            Context.OwnerTask = x => selectaction((T)x); // Simple way of Converting T to specific type (object in this case)
            Context.ContentContext = content.DataContext as IPopup;
            Context.CloseWindow = () => popupWindow.Close();
            Context.Title = title;
            Context.Ready();

            return window.Invoke(() => window.ShowChildWindowAsync(popupWindow));
        }
        public Task ShowSelectPopupNoButtonAsync<T>(object context, string title, UserControl content, Action<T> selectaction)
        {
            var window = GetMetroWindow(context);

            // Create BaseChildWindow
            var popupWindow = new SelectPopupWindowNoButton() { IsModal = true, AllowMove = true, AdditionalContent = content };

            // Fill Context with values so ViewModels are linked  // TODO Improve, let Injection handle more
            var Context = (PopupViewModel)popupWindow.DataContext;
            Context.OwnerTask = x => selectaction((T)x); // Simple way of Converting T to specific type (object in this case)
            Context.ContentContext = content.DataContext as IPopup;
            Context.CloseWindow = () => popupWindow.Close();
            Context.Title = title;
            Context.Ready();

            return window.Invoke(() => window.ShowChildWindowAsync(popupWindow));
        }

        private static MetroWindow GetMetroWindow(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (!PopupParticipation.IsRegistered(context))
            {
                throw new InvalidOperationException("Context is not registered. Consider using ChildWindowParticipation. Register in XAML to bind in the DataContext.");
            }

            var association = PopupParticipation.GetAssociation(context);
            var metroWindow = association.Invoke(() => Window.GetWindow(association) as MetroWindow);
            if (metroWindow == null)
            {
                throw new InvalidOperationException("Context is not inside a MetroWindow.");
            }
            return metroWindow;
        }
    }
}
