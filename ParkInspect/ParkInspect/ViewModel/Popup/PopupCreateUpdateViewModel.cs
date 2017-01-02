using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.ViewModel.Popup
{
    public class PopupCreateUpdateViewModel : PopupViewModel
    {
        public ICreateUpdatePopup ContentContextNewUpdate { get; set; }

        private void Confirm()
        {
            OwnerTask(ContentContextNewUpdate.SelectedItemPopup);
            CloseWindow();
        }

        public override void Ready()
        {
            base.Ready();
            ContentContextNewUpdate.PopupDone = Confirm;
        }
    }
}
