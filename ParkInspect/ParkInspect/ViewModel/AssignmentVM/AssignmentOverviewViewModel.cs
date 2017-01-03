using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel.Popup;

namespace ParkInspect.ViewModel.AssignmentVM
{
    public class AssignmentOverviewViewModel : ViewModelBase
    {
        private readonly AssignmentService _service;

        private ObservableCollection<AssignmentViewModel> Data { get; set; }

        private AssignmentViewModel _selectedAssignment;

        public ObservableCollection<AssignmentViewModel> Assignments { get; set; }

        public AssignmentViewModel SelectedAssignment
        {
            get { return _selectedAssignment; }
            set { Set(ref _selectedAssignment, value); }
        }

        #region Filter Properties

        private string _clientfilter;
        public string ClientFilter
        {
            get { return _clientfilter; }
            set
            {
                _clientfilter = value;
                UpdateFilter();
            }
        }

        private string _datefilter;
        public string DateFilter
        {
            get { return _datefilter; }
            set
            {
                _datefilter = value;
                UpdateFilter();
            }
        }

        private string _statefilter;
        public string StateFilter
        {
            get { return _statefilter; }
            set
            {
                _statefilter = value;
                UpdateFilter();
            }
        }

        private string _deadlineFilter;
        public string DeadlineFilter
        {
            get { return _deadlineFilter; }
            set
            {
                _deadlineFilter = value;
                UpdateFilter();
            }
        }

        private string _clarificationFilter;
        public string ClarificationFilter
        {
            get { return _clarificationFilter; }
            set
            {
                _clarificationFilter = value;
                UpdateFilter();
            }
        }
        #endregion

        public RelayCommand NewCommand { get; set; }

        public AssignmentOverviewViewModel(IRepository context, PopupManager popup, DialogManager dialog)
        {
            _service = new AssignmentService(context);

            Data = new ObservableCollection<AssignmentViewModel>(_service.GetAll<Asignment>().Select(x => new AssignmentViewModel(context, x, popup, dialog)));
            Assignments = Data;

            NewCommand = new RelayCommand(() => NewAssignment(context, popup, dialog));
            NewAssignment(context, popup, dialog);
        }

        private void NewAssignment(IRepository context, PopupManager popup, DialogManager dialog)
        {
            SelectedAssignment = new AssignmentViewModel(context, new Asignment(), popup, dialog);
            RaisePropertyChanged();
        }

        private void UpdateFilter()
        {
            var builder = new FilterBuilder();
            builder.Add("Client.name", ClientFilter);
            builder.Add("State.state1", StateFilter);
            builder.Add("Date", DateFilter);
            builder.Add("Deadline", DeadlineFilter);
            builder.Add("Clarification", ClarificationFilter);

            var result = Data.Where(a => a.Like(builder.Get()));

            Assignments = new ObservableCollection<AssignmentViewModel>(result);
            RaisePropertyChanged("Assignments");
        }

    }
}
