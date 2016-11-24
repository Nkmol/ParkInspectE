using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PersoneelViewModel : ViewModelBase
    {
        #region fields_and_properties
        //Fields and Properties employee
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private string _firstname;
        public string Firstname
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                base.RaisePropertyChanged();
            }
        }

        private string _lastname;
        public string Lastname
        {
            get
            {
                return _lastname;
            }
            set
            {
                _lastname = value;
                base.RaisePropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                base.RaisePropertyChanged();
            }
        }

        private string _emailadres;
        public string Emailadres
        {
            get
            {
                return _emailadres;
            }
            set
            {
                _emailadres = value;
                base.RaisePropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; base.RaisePropertyChanged(); }
        }

        private DateTime _started;
        public DateTime Started
        {
            get
            {
                return _started;
            }
            set
            {
                _started = value;
                base.RaisePropertyChanged();
            }
        }

        private DateTime _ended;
        public DateTime Ended
        {
            get
            {
                return _ended;
            }
            set
            {
                _ended = value;
                base.RaisePropertyChanged();
            }
        }

        private string _role;
        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                base.RaisePropertyChanged();
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                base.RaisePropertyChanged();
            }
        }

        public string BoundMessage { get; set; }

        const string Msg1 = "Actief, datum uit dienst wordt niet verwerkt.";
        const string Msg2 = "Niet actief, voer datum uit dienst correct in.";
     
        private bool _active = false;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                BoundMessage = _active ? Msg1 : Msg2;
                base.RaisePropertyChanged(() => BoundMessage);
                base.RaisePropertyChanged("Actice");
            }
        }

        private string _notification;
        public string Notification
        {
            get { return _notification; }
            set
            {
                _notification = value;
                base.RaisePropertyChanged();
            }
        }
    #endregion

        //Data for comboBoxes
        public ObservableCollection<Role> RoleCollection { get; set; }
        public ObservableCollection<Employee_Status> StatusCollection { get; set; }

        //Data Employees
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        { get { return _employees; } set { _employees = value; base.RaisePropertyChanged(); } }
        public EmployeeService Service { get; set; }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee == value) return;

                _selectedEmployee = value;
                UpdateSelectedItem();
            }
        }

        //Commands
        public ICommand CreateItemCommand { get; set; }
        public ICommand EditItemCommand { get; set; }
        public ICommand DeselectEmployeeCommand { get; set; }

        public PersoneelViewModel(IRepository context)
        {
            //Service and employees
            Service = new EmployeeService(context);
            Employees = new ObservableCollection<Employee>(Service.GetAllEmployees());

            //Collections for comboboxes
            RoleCollection = new ObservableCollection<Role>(Service.GetAllRoles());
            StatusCollection = new ObservableCollection<Employee_Status>(Service.GetAllStatusses());

            Started = DateTime.Today;
            Ended = DateTime.Today;
            
            CreateItemCommand = new RelayCommand(CreateNewEmployee, CanCreate);
            EditItemCommand = new RelayCommand(EditEmployee, CanEdit);
            DeselectEmployeeCommand = new RelayCommand(DeselectItem, CanDeselect);
        }

        //CRU METHODS
        private void CreateNewEmployee()
        {
            CreateOrUpdate(true);
        }

        private bool CanCreate()
        {
            return SelectedEmployee == null;
        }

        private void EditEmployee()
        {
            CreateOrUpdate(false);
        }

        private bool CanEdit()
        {
            return SelectedEmployee != null;
        }

        private void CreateOrUpdate(bool create)
        {
            Employee employee;

            if (create)
            {
                employee = new Employee();
            }
            else
            {
                employee = SelectedEmployee;
            }

            employee.firstname = Firstname;
            employee.lastname = Lastname;
            employee.employee_status = Status;
            employee.in_service_date = Started;

            //Checking if out-of-service date has to be set in the database
            if (!Active)
            {
                if (Started < Ended)
                {
                    employee.out_service_date = Ended;
                }
            }
            else
            {
                employee.out_service_date = null;
            }

            employee.role = Role;
            employee.password = Password;
            employee.email = Emailadres;
            employee.phonenumber = PhoneNumber;
            employee.active = Active;

            if (create)
            {
                Service.InsertEntity(employee);
                Notification = "De medewerker is opgeslagen";
            }
            else
            {
                Service.UpdateEntity(employee);
                Notification = "De medewerker is aangepast";
            }
            UpdateDataGrid();
        }

        //OTHER METHODS
        private void DeselectItem()
        {
            SelectedEmployee = null;
            ResetProperties();
        }

        private bool CanDeselect()
        {
            return SelectedEmployee != null;
        }

        private void UpdateDataGrid()
        {
            SelectedEmployee = null;
             _employees = new ObservableCollection<Employee>(Service.GetAllEmployees());
            var temp = Employees;
            Employees = null;
            Employees = temp;
            ResetProperties();
        }

        private void UpdateSelectedItem()
        {
            if (SelectedEmployee != null)
            {
                Firstname = SelectedEmployee.firstname;
                Lastname = SelectedEmployee.lastname;
                Password = SelectedEmployee.password;
                Started = SelectedEmployee.in_service_date;

                if (SelectedEmployee.out_service_date != null)
                {
                    Ended = SelectedEmployee.out_service_date.Value;
                }

                Active = SelectedEmployee.active;
                Emailadres = SelectedEmployee.email;
                PhoneNumber = SelectedEmployee.phonenumber;
                Role = SelectedEmployee.role;
                Status = SelectedEmployee.employee_status;
            }
        }

        private void ResetProperties()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Started = DateTime.Today;
            Emailadres = string.Empty;
            PhoneNumber = string.Empty;
            Active = false;
            Password = string.Empty;
        }
    }
}