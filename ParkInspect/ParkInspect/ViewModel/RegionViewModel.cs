using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class RegionViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;

        private string _message;

        private Region _newRegion;

        private string _regionFilter;

        private ObservableCollection<Region> _regions;
        private Region _selectedRegion;
        protected RegionService Service;

        public RegionViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new RegionService(context);
            Data = Service.GetAll<Region>();

            Regions = new ObservableCollection<Region>(Service.GetAll<Region>());
            NewRegion = new Region();

            SaveNewRegionCommand = new RelayCommand(SaveNewRegionMethod);
            DeleteRegionCommand = new RelayCommand(DeleteRegionMethod);
        }

        private IEnumerable<Region> Data { get; set; }

        public ICommand SaveNewRegionCommand { get; set; }
        public ICommand DeleteRegionCommand { get; set; }

        public ObservableCollection<Region> Regions
        {
            get { return _regions; }
            set
            {
                _regions = value;
                base.RaisePropertyChanged();
            }
        }

        public Region NewRegion
        {
            get { return _newRegion; }
            set
            {
                _newRegion = value;
                RaisePropertyChanged();
            }
        }

        public Region SelectedRegion
        {
            get { return _selectedRegion; }

            set
            {
                _selectedRegion = value;
                base.RaisePropertyChanged();
            }
        }

        public string RegionFilter
        {
            get { return _regionFilter; }
            set
            {
                _regionFilter = value;
                Filter();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                base.RaisePropertyChanged();
            }
        }

        private void SaveNewRegionMethod()
        {
            if (NewRegion?.name == null)
                return;
            Message = Service.Add(NewRegion) ? "Een nieuwe regio is opgeslagen!" : "Something went wrong.";
            _dialog.ShowMessage("Succes!", Message);

            Regions.Add(NewRegion);
            base.RaisePropertyChanged();

            NewRegion = new Region();
        }

        private void DeleteRegionMethod()
        {
            if (SelectedRegion == null)
            {
                Message = Service.Delete(SelectedRegion) ? "Something went wrong." : "Selecteer een regio!";
                _dialog.ShowMessage("Er ging iets fout!", Message);
                return;
            }

            var dialogResult = MessageBox.Show("Weet u zeker dat  u deze regio wilt verwijderen?", "Verwijderen",
                MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                Service.Delete(SelectedRegion);
                Regions.Remove(SelectedRegion);
                base.RaisePropertyChanged();
            }
        }

        private void Filter()
        {
            Data = Service.GetAll<Region>();

            var builder = new FilterBuilder();
            builder.Add("name", RegionFilter);

            var filters = builder.Get();
            var result = Data.Where(a => a.Like(filters));

            Regions = new ObservableCollection<Region>(result);
            RaisePropertyChanged("Regions");
        }
    }
}