using System;
using System.Collections.ObjectModel;
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

        //Data Emplooyees
        public ObservableCollection<Employee> Employees { get; set; }
        public EmployeeService Service { get; set; }
        public Employee SelectedEmployee { get; set; }

        //Commands
        public  ICommand CreateItemCommand { get; set; }

        
        public PersoneelViewModel(IRepository context)
        {
            Service = new EmployeeService(context);
            Employees = new ObservableCollection<Employee>(Service.GetAllEmployees());

            CreateItemCommand = new RelayCommand(CreateNewEmployee);
        }

        private void CreateNewEmployee()
        {
            DateTime testDateTime = new DateTime();
            testDateTime = DateTime.Today;
            Employee newEmployee = new Employee();
            
            newEmployee.firstname = "FREEK";
            newEmployee.lastname = "WAZAA";
            newEmployee.employee_status = "beschikbaar";
            newEmployee.in_service_date = testDateTime;
            newEmployee.out_service_date = null;
            newEmployee.role = "manager";
            newEmployee.password = "test";
            newEmployee.email = "freekwazaa@parkinspect.nl";
            newEmployee.phonenumber = "01234567891";
            newEmployee.active = true;

            Service.InsertEntity(newEmployee);
        }
    }
}