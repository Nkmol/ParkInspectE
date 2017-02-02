using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.Region
{
    public class RegionViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;
        private readonly ParkInspect.Region Data;
        protected RegionService Service;

        public RegionViewModel(IRepository context, ParkInspect.Region data, DialogManager dialog)
        {
            _dialog = dialog;
            Data = data;
            Service = new RegionService(context);
            SaveCommand = new RelayCommand<RegionOverviewViewModel>(Save);
            DeleteCommand = new RelayCommand<RegionOverviewViewModel>(Delete);
            Reset();

            FillForm();
        }

        #region ViewModel Poco properties      

        public string Name
        {
            get { return Data.name; }
            set
            {
                Data.name = value;
            }
        }
       
        #endregion

        #region Property Form

        private string _formName;

        public string FormName
        {
            get { return _formName; }
            set { Set(ref _formName, value); }
        }

        #endregion

        public string Message { get; set; }
        public bool CanDelete => Data?.name != null;

        public RelayCommand<RegionOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand<RegionOverviewViewModel> DeleteCommand { get; set; }

        private void SaveForm()
        {
            Name = FormName;
        }

        private void FillForm()
        {
            FormName = Name;
        }

        public void Reset()
        {
            FillForm();
        }

        private void Save(RegionOverviewViewModel overview)
        {
            if (overview.Regions.Contains(this))
                Edit();
            else
                Add(overview);

            overview.NewRegion();
        }

        public void Add(RegionOverviewViewModel overview)
        {
            SaveForm();

            var rs = Service.Add(Data);

            Message = rs ? "De regio is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            _dialog.ShowMessage("Regio toevoegen", Message);

            if(rs)
                overview.Regions.Add(this);
        }

        public void Edit()
        {
            SaveForm();
            Message = Service.Update(Data) ? "De regio is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Regio bewerken", Message);
        }

        private void Delete(RegionOverviewViewModel overview)
        {
            Message = Service.Delete(Data) ? "De regio is verwijderd!" : "Er is iets misgegaan tijdens het verwijderen.";

            _dialog.ShowMessage("Regio verwijderen", Message);
            overview.Regions.Remove(this);
            overview.NewRegion();
        }
    }
}