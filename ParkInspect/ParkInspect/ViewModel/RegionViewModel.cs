using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;

namespace ParkInspect.ViewModel
{
    public class RegionViewModel : ViewModelBase
    {
        private readonly Region _region;

        #region POCO Properties

        public string Name
        {
            get { return _region.name;  }
            set
            {
                _region.name = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        // Does not need its own Service, so no need to pass Repository
        public RegionViewModel(Region region)
        {
            _region = region;
        }
    }
}
