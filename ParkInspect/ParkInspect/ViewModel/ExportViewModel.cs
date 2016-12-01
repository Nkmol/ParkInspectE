using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ExportViewModel
    {

        public List<string> Tables;

        public ExportViewModel(IRepository context)
        {

            RegisterTables();

        }

        private void RegisterTables()
        {
            
        }

    }
}
