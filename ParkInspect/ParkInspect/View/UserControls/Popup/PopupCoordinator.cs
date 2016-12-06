using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace ParkInspect.View.UserControls.Popup
{
    public class PopupCoordinator
    {
        /// <summary>
        /// Gets the default instance if the popup coordinator, which can be injected into a view model.
        /// </summary>
        public static readonly PopupCoordinator Instance = new PopupCoordinator();

        public Task ShowPopupAsync(object context, string title)
        {
            var window = GetMetroWindow(context);
            return window.Invoke(() => window.ShowChildWindowAsync(new BaseChildWindow() { IsModal = true, AllowMove = true}));
        }

        private static MetroWindow GetMetroWindow(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (!ChildWindowParticipation.IsRegistered(context))
            {
                throw new InvalidOperationException("Context is not registered. Consider using ChildWindowParticipation.Register in XAML to bind in the DataContext.");
            }

            var association = ChildWindowParticipation.GetAssociation(context);
            var metroWindow = association.Invoke(() => Window.GetWindow(association) as MetroWindow);
            if (metroWindow == null)
            {
                throw new InvalidOperationException("Context is not inside a MetroWindow.");
            }
            return metroWindow;
        }
    }
}
