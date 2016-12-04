using GalaSoft.MvvmLight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model;

namespace ParkInspect.ViewModel
{
    public class ExportViewModel : ViewModelBase
    {

        public IEnumerable Data { get; set; }

        public RelayCommand ExportCommand { get; set; }

        public ExportViewModel()
        {
            
            ExportCommand = new RelayCommand(Export);

        }

        private void Export()
        {

            ExportFactory.ExportPdf(Data.Cast<dynamic>());

        }
       
    }
}
