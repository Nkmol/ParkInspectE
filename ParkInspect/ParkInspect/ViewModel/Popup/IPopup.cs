using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.ViewModel
{
    public interface IPopup
    {
        // Property to give back to caller screen
        object SelectedItemPopup { get; }
    }
}
