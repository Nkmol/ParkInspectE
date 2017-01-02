using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace ParkInspect.ViewModel.Popup
{
    public interface ICreateUpdatePopup : IPopup
    {
        Action PopupDone { get; set; }
    }
}
