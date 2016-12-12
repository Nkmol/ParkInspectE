using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

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

        public ObservableCollection<Object> EmployeeCollection { get; set; }

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
            if (SelectedEmployee.active)
                SelectedEmployee.out_service_date = null;

            Service.InsertEntity(SelectedEmployee);
            Notification = "De medewerker is opgeslagen";
            UpdateDataGrid();
        }

        private void EditEmployee()
        {
            if (SelectedEmployee.active)
                SelectedEmployee.out_service_date = null;

            Service.UpdateEntity(SelectedEmployee);
            Notification = "De medewerker is aangepast";
            UpdateDataGrid();
        }

        /// <summary>
        /// Initializes a new instance of the PersoneelViewModel class.
        /// </summary>
        public EmployeeViewModel()
        {

        private void UpdateDataGrid()
        {
             _employees = new ObservableCollection<Employee>(Service.GetAllEmployees());
            var temp = Employees;
            Employees = null;
            Employees = temp;
            SetNewEmployee();
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