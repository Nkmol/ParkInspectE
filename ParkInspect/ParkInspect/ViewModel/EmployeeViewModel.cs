using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.Specialized;
using System.Security.Cryptography;
using ParkInspect;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EmployeeViewModel : ViewModelBase
    {
        //Service
        protected EmployeeService Service { get; set; }
        private string oldPass;

        public ObservableCollection<Absence> Absences { get; set; }

        //Fields and Properties
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
                Filter();
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
                Filter();
            }
        }

        private DialogManager _dialog;

        //Data for comboBoxes
        public ObservableCollection<Role> RoleCollection { get; set; }
        public ObservableCollection<Employee_Status> StatusCollection { get; set; }

        //Data Employees
        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                _employees = value;
                base.RaisePropertyChanged();
            }
        }

        private IEnumerable<Employee> Data { get; set; }


        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                Set(ref _selectedEmployee, value);
                base.RaisePropertyChanged();
                if (_selectedEmployee != null)
                {
                    oldPass = _selectedEmployee.password;
                }

                if(value != null)
                    Absences = new ObservableCollection<Absence>(SelectedEmployee.Absences.ToList());
            }
        }

        //Commands
        public ICommand SaveCommand { get; set; }
        public ICommand DeselectEmployeeCommand { get; set; }

        public EmployeeViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            //Service and employees
            Service = new EmployeeService(context);
            Employees = new ObservableCollection<Employee>(Service.GetAllEmployees());
            Data = Service.GetAllEmployees();

            //Initialize startup Employee
            SelectedEmployee = new Employee();
            SelectedEmployee.in_service_date = DateTime.Today;
            SelectedEmployee.out_service_date = DateTime.Today;

            //Collections for comboboxes
            RoleCollection = new ObservableCollection<Role>(Service.GetAll<Role>());
            StatusCollection = new ObservableCollection<Employee_Status>(Service.GetAll<Employee_Status>());

            //Initialize commands
            SaveCommand = new RelayCommand(SaveEmployee);
            DeselectEmployeeCommand = new RelayCommand(SetNewEmployee);
        }


        //CRU METHODS
        private void SaveEmployee()
        {
            bool error = false;

            //Checking all combinations between 'Active' AND 'status' for not possible combinations.
            //Using a boolean error to trigger error message and popup the error dialog.

            if (SelectedEmployee.active)
            {
                if (SelectedEmployee.employee_status.Equals("Retired"))
                {
                    Notification = "Een medewerker kan niet 'actief' zijn als hij/zij met pensioen is.";
                    error = true;
                }
                else if (SelectedEmployee.employee_status.Equals("Terminated"))
                {
                    Notification = "Een medewerker kan niet 'actief' zijn als hij/zij ontslagen is.";
                    error = true;
                }

                SelectedEmployee.out_service_date = null;
            }
            else
            {
                if (SelectedEmployee.employee_status.Equals("Available"))
                {
                    Notification = "Een medewerker kan niet 'beschikbaar' zijn als hij/zij geen lopend contract heeft.";
                    error = true;
                }

                if (SelectedEmployee.employee_status.Equals("On Non-Pay leave"))
                {
                    Notification =
                        "Een medewerker kan niet 'Op betaald verlof' zijn als hij/zij geen lopend contract heeft.";
                    error = true;
                }

                if (SelectedEmployee.employee_status.Equals("Suspended"))
                {
                    Notification = "Een medewerker kan niet 'Geschorst' zijn als hij/zij geen lopend contract heeft.";
                    error = true;
                }

                if (SelectedEmployee.out_service_date.Equals(null))
                {
                    Notification = "Er moet een datum uit dienst ingevoerd worden";
                    error = true;
                }
            }

            if (error)
            {
                _dialog.ShowMessage("Fout opgetreden", Notification);
                return;
            }

            /*
            SHA256 sha = SHA256.Create();

            byte[] bytes = new byte[SelectedEmployee.password.Length * sizeof(char)];
            System.Buffer.BlockCopy(SelectedEmployee.password.ToCharArray(), 0, bytes, 0, bytes.Length);

            sha.ComputeHash(bytes);

            char[] chars = new char[sha.Hash.Length / sizeof(char)];
            System.Buffer.BlockCopy(sha.Hash, 0, chars, 0, sha.Hash.Length);

            SelectedEmployee.password = new string(chars);

            Service.Add(SelectedEmployee);

            SelectedEmployee.password = new string(chars);
            */
            if (SelectedEmployee.id == 0)
            {
                Service.Add(SelectedEmployee);
            }
            else
            {
                Service.Update(SelectedEmployee);
                Notification = "De medewerker is aangepast";
            }
            
            _dialog.ShowMessage("Gelukt!", Notification);
            UpdateDataGrid();
        }

        /// <summary>
        /// Initializes a new instance of the PersoneelViewModel class.
        /// </summary>

        private void UpdateDataGrid()
        {
            _employees = new ObservableCollection<Employee>(Service.GetAllEmployees());
            var temp = Employees;
            Employees = null;
            Employees = temp;
            SetNewEmployee();
        }

        private void Filter()
        {
            var builder = new FilterBuilder();
            builder.Add("firstname", FirstNameFilter);
            builder.Add("lastname", LastNameFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Employees = new ObservableCollection<Employee>(result);
            RaisePropertyChanged("Employees");
        }

        private void SetNewEmployee()
        {
            Employee e = new Employee();
            e.in_service_date = DateTime.Today;
            e.out_service_date = DateTime.Today;

            SelectedEmployee = e;
        }
    }
}