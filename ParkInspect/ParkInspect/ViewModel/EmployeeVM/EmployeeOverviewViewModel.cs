using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.EmployeeVM
{
    public class EmployeeOverviewViewModel : ViewModelBase
    {
        private readonly EmployeeService _service;

        private ObservableCollection<EmployeeViewModel> Data { get; set; }

        private EmployeeViewModel _selectedEmployee;

        public ObservableCollection<EmployeeViewModel> Employees { get; set; }

        public EmployeeViewModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                Set(ref _selectedEmployee, value);
                SelectedEmployee?.Reset();
            }
        }

        public EmployeeOverviewViewModel(IRepository context, DialogManager dialog)
        {
            _context = context;
            _dialog = dialog;
            _service = new EmployeeService(context);

            Data = new ObservableCollection<EmployeeViewModel>(_service.GetAll<Employee>().Select(x => new EmployeeViewModel(context, x, dialog)));
            Employees = Data;

            NewCommand = new RelayCommand(NewEmployee);
            NewEmployee();
        }

        #region Filters
        private string _firstNameFilter;

        public string FirstNameFilter
        {
            get
            {
                return _firstNameFilter;
            }
            set
            {
                _firstNameFilter = value;
                UpdateFilter();
            }
        }

        private string _lastNameFilter;

        public string LastNameFilter
        {
            get
            {
                return _lastNameFilter;
            }
            set
            {
                _lastNameFilter = value;
                UpdateFilter();
            }
        }

        private string _phoneFilter;

        public string PhoneFilter
        {
            get
            {
                return _phoneFilter;
            }
            set
            {
                _phoneFilter = value;
                UpdateFilter();
            }
        }

        private string _emailFilter;

        public string EmailFilter
        {
            get
            {
                return _emailFilter;
            }
            set
            {
                _emailFilter = value;
                UpdateFilter();
            }
        }

        private string _statusFilter;

        public string StatusFilter
        {
            get
            {
                return _statusFilter;
            }
            set
            {
                _statusFilter = value;
                UpdateFilter();
            }
        }

        private string _roleFilter;

        public string RoleFilter
        {
            get
            {
                return _roleFilter;
            }
            set
            {
                _roleFilter = value;
                UpdateFilter();
            }
        }

        private string _inServiceFilter;

        public string InServiceFilter
        {
            get
            {
                return _inServiceFilter;
            }
            set
            {
                _inServiceFilter = value;
                UpdateFilter();
            }
        }

        private string _outServiceFilter;

        public string OutServiceFilter
        {
            get
            {
                return _outServiceFilter;
            }
            set
            {
                _outServiceFilter = value;
                UpdateFilter();
            }
        }

        private string _activeFilter;

        public string ActiveFilter
        {
            get
            {
                return _activeFilter;
            }
            set
            {
                _activeFilter = value;
                UpdateFilter();
            }
        }

        #endregion

        public RelayCommand NewCommand { get; set; }

        private readonly IRepository _context;
        private readonly DialogManager _dialog;

        public void NewEmployee()
        {
            SelectedEmployee = new EmployeeViewModel(_context, new Employee(), _dialog);
            RaisePropertyChanged();
        }

        public void EmployeesChanged()
        {
            Data = new ObservableCollection<EmployeeViewModel>(_service.GetAll<Employee>().Select(x => new EmployeeViewModel(_context, x, _dialog)));
            Employees = Data;
            RaisePropertyChanged("Employees");
        }

        private void UpdateFilter()
        {
            var builder = new FilterBuilder();
            builder.Add("Firstname", FirstNameFilter);
            builder.Add("Lastname", LastNameFilter);
            builder.Add("Phonenumber", PhoneFilter);
            builder.Add("InServiceDate", InServiceFilter);
            builder.Add("OutServiceDate", OutServiceFilter);
            builder.Add("Email", EmailFilter);
            builder.Add("Active", ActiveFilter);
            builder.Add("Role.role1", RoleFilter);
            builder.Add("EmployeeStatus.employee_status1", StatusFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Employees = new ObservableCollection<EmployeeViewModel>(result);
            RaisePropertyChanged("Employees");
        }
    }
}
