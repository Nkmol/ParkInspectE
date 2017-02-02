using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;

namespace ParkInspect.ViewModel.AbsenceVM
{
    public class AbsenceOverviewViewModel : ViewModelBase
    {
        private readonly IRepository _context;
        private readonly DialogManager _dialog;

        private AbsenceViewModel _selectedAbsence;

        public AbsenceOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;

            Data =
                new ObservableCollection<AbsenceViewModel>(
                    context.GetAll<Absence>().Select(x => new AbsenceViewModel(context, x, dialog)));
            Absences = Data;

            NewCommand = new RelayCommand(NewAbsence);
            NewAbsence();
        }

        private ObservableCollection<AbsenceViewModel> Data { get; }

        public ObservableCollection<AbsenceViewModel> Absences { get; set; }

        public AbsenceViewModel SelectedAbsence
        {
            get { return _selectedAbsence; }
            set
            {
                Set(ref _selectedAbsence, value);
                SelectedAbsence?.Reset();
            }
        }

        public RelayCommand NewCommand { get; set; }

        public void NewAbsence()
        {
            SelectedAbsence = new AbsenceViewModel(_context, new Absence(), _dialog);
            RaisePropertyChanged("Absences");
        }

        private void UpdateAbsences()
        {
            var builder = new FilterBuilder();
            builder.Add("Employee.firstname", FirstnameFilter);
            builder.Add("Employee.lastname", LastnameFilter);
            builder.Add("Start", StartFilter);
            builder.Add("End", EndFilter);

            var result = Data.Where(a => a.Like(builder.Get()));

            Absences = new ObservableCollection<AbsenceViewModel>(result);
            RaisePropertyChanged("Absences");
        }

        #region Filters

        private string _startFilter;

        public string StartFilter
        {
            get { return _startFilter; }
            set
            {
                _startFilter = value;
                UpdateAbsences();
            }
        }

        private string _endFilter;

        public string EndFilter
        {
            get { return _endFilter; }
            set
            {
                _endFilter = value;
                UpdateAbsences();
            }
        }

        private string _firstnameFilter;

        public string FirstnameFilter
        {
            get { return _firstnameFilter; }
            set
            {
                _firstnameFilter = value;
                UpdateAbsences();
            }
        }

        private string _lastnameFilter;

        public string LastnameFilter
        {
            get { return _lastnameFilter; }
            set
            {
                _lastnameFilter = value;
                UpdateAbsences();
            }
        }

        #endregion
    }
}
