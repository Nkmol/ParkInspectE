using GalaSoft.MvvmLight;

namespace ParkInspect.ViewModel.Regio
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class RegionViewModel : ViewModelBase
    {
        private readonly Region _region;

        public string Name
        {
            get { return _region.name; }
            set { _region.name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Initializes a new instance of the RegioViewModel class.
        /// </summary>
        public RegionViewModel(Region region)
        {
            this._region = region;
        }
    }
}