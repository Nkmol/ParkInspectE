using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace ParkInspect.ViewModel
{
    
    public class NewTemplateViewModel : ViewModelBase
    {
        // public Template template;
        private VragenlijstViewModel superViewModel;
        public string fieldLabel { get; set; }

        public NewTemplateViewModel(VragenlijstViewModel superViewModel)
        {
            this.superViewModel = superViewModel;
        }

        public void setTemplate(/* Template template */)
        {
            // this.template = template;
        }
    }
}
