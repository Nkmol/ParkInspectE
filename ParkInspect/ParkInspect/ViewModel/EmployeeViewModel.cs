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
    public class EmployeeViewModel : ViewModelBase
    {
        //Service
        protected EmployeeService Service { get; set; }

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
        

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                Set(ref _selectedEmployee, value);
                base.RaisePropertyChanged();
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

            SelectedEmployee = new Employee();
            SelectedEmployee.in_service_date = DateTime.Today;
            SelectedEmployee.out_service_date = DateTime.Today;

            //Collections for comboboxes
            RoleCollection = new ObservableCollection<Role>(Service.GetAllRoles());
            StatusCollection = new ObservableCollection<Employee_Status>(Service.GetAllStatusses());
            
            CreateItemCommand = new RelayCommand(CreateNewEmployee);
            EditItemCommand = new RelayCommand(EditEmployee);
            DeselectEmployeeCommand = new RelayCommand(SetNewEmployee);
        }

        //CRU METHODS
        private void CreateNewEmployee()
        {
            if(SelectedEmployee.firstname == null || SelectedEmployee.lastname == null || 
                SelectedEmployee.email == null || SelectedEmployee.role == null || 
                SelectedEmployee.password == null || SelectedEmployee.employee_status == null ||
                SelectedEmployee.phonenumber == null)
                return;

            if (SelectedEmployee.active)
                SelectedEmployee.out_service_date = null;

            Service.InsertEntity(SelectedEmployee);
            Notification = "De medewerker is opgeslagen";
            UpdateDataGrid();
        }

        private void EditEmployee()
        {
            if (SelectedEmployee.firstname == null || SelectedEmployee.lastname == null ||
                SelectedEmployee.email == null || SelectedEmployee.role == null ||
                SelectedEmployee.password == null || SelectedEmployee.employee_status == null ||
                SelectedEmployee.phonenumber == null)
                return;

            if (SelectedEmployee.active)
                SelectedEmployee.out_service_date = null;

            Service.UpdateEntity(SelectedEmployee);
            Notification = "De medewerker is aangepast";
            UpdateDataGrid();
        }

        /// <summary>
        /// Initializes a new instance of the PersoneelViewModel class.
        /// </summary>
        public PersoneelViewModel()
        {
            Employee e = new Employee();
            e.in_service_date = DateTime.Today;
            e.out_service_date = DateTime.Today;
            SelectedEmployee = e;
        }

        private void UpdateDataGrid()
        {
            SetNewEmployee();
             _employees = new ObservableCollection<Employee>(Service.GetAllEmployees());
            var temp = Employees;
            Employees = null;
            Employees = temp;
        }
    }
}