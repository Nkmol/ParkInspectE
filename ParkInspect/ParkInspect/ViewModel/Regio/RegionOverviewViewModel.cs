using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.Regio
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class RegionOverviewViewModel : ViewModelBase, IPopup
    {
        private ParkinglotService _service;
        public ObservableCollection<RegionViewModel> Regions { get; set; }
        public RegionViewModel SelectedRegion { get; set; }
        // Same as SelectedRegion
        public object SelectedItemPopup => SelectedRegion;

        /// <summary>
        /// Initializes a new instance of the RegioOverviewViewModel class.
        /// </summary>
        public RegionOverviewViewModel(IRepository context)
        {
            _service = new ParkinglotService(context);
            Regions = new ObservableCollection<RegionViewModel>(_service.GetAllRegions().Select(x => new RegionViewModel(x)));
        }
    }
}