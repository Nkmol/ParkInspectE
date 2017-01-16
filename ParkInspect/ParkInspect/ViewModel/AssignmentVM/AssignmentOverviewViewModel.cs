using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Org.BouncyCastle.Crypto.Digests;
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
            set
            {
                Set(ref _selectedAssignment, value);
                if (SelectedAssignment != null)
                    SelectedAssignment.Reset();
            }
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

        private readonly IRepository _context;
        private readonly PopupManager _popup;
        private readonly DialogManager _dialog;
        public AssignmentOverviewViewModel(IRepository context, PopupManager popup, DialogManager dialog)
        {
            _context = context;
            _popup = popup;
            _dialog = dialog;
            _service = new AssignmentService(context);

            Data = new ObservableCollection<AssignmentViewModel>(_service.GetAll<Asignment>().Select(x => new AssignmentViewModel(context, x, popup, dialog)));
            Assignments = Data;
            
            NewCommand = new RelayCommand(NewAssignment);
            NewAssignment();
        }

        private void NewAssignment()
        {
            SelectedAssignment = new AssignmentViewModel(_context, new Asignment(), _popup, _dialog);
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
