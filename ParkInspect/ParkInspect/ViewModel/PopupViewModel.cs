﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ParkInspect.ViewModel
{
    public class PopupViewModel : ViewModelBase
    {
        public IPopup ContentContext { get; set; } // TODO IPopupParticipate
        public Action<object> OwnerTask { get; set; }
        public RelayCommand SelectCommand { get; set; }
        public Action CloseWindow { get; set; }
        public PopupViewModel()
        {
            SelectCommand = new RelayCommand(SelectEntity, () => OwnerTask != null); // TODO If SelectedItem != null
        }

        private void SelectEntity()
        {
            OwnerTask(ContentContext.SelectedItemPopup);
            CloseWindow(); // Close popup after something has been selected
        }

        public void Ready()
        {
            SelectCommand.RaiseCanExecuteChanged();
        }
    }
}
