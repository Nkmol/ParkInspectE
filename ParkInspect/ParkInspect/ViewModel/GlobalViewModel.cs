using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class GlobalViewModel : ViewModelBase
    {
        private readonly DataService _serivce;

        private ObservableCollection<Form> _forms;
        public ObservableCollection<Form> Forms
        {
            get { return _forms ?? (_forms = new ObservableCollection<Form>(_serivce.GetAll<Form>())); }
            set { _forms = value; }
        }

        private ObservableCollection<State> _states;
        public ObservableCollection<State> States
            {
            get { return _states ?? (_states = new ObservableCollection<State>(_serivce.GetAll<State>())); }
            set { _states = value; }
        }

        private ObservableCollection<Employee_Status> _employeeStates;
        public ObservableCollection<Employee_Status> EmployeeStates
        {
            get { return _employeeStates ?? (_employeeStates = new ObservableCollection<Employee_Status>(_serivce.GetAll<Employee_Status>())); }
            set { _employeeStates = value; }
        }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get { return _clients ?? (_clients = new ObservableCollection<Client>(_serivce.GetAll<Client>())); }
            set { _clients = value; }
        }

        private ObservableCollection<Asignment> _assignments;
        public ObservableCollection<Asignment> Assignments
        {
            get { return _assignments ?? (_assignments = new ObservableCollection<Asignment>(_serivce.GetAll<Asignment>())); }
            set { _assignments = value; }
        }

        private ObservableCollection<Contactperson> _contactpersons;
        public ObservableCollection<Contactperson> Contactpersons
        {
            get { return _contactpersons ?? (_contactpersons = new ObservableCollection<Contactperson>(_serivce.GetAll<Contactperson>())); }
            set { _contactpersons = value; }
        }

        private ObservableCollection<Role> _roles;
        public ObservableCollection<Role> Roles
        {
            get { return _roles ?? (_roles = new ObservableCollection<Role>(_serivce.GetAll<Role>())); }
            set { _roles = value; }
        }

        private ObservableCollection<Inspection> _inspections;
        public ObservableCollection<Inspection> Inspections
        {
            get { return _inspections ?? (_inspections = new ObservableCollection<Inspection>(_serivce.GetAll<Inspection>())); }
            set { _inspections = value; }
        }

        private ObservableCollection<Parkinglot> _parkinglots;
        public ObservableCollection<Parkinglot> Parkinglots
        {
            get { return _parkinglots ?? (_parkinglots = new ObservableCollection<Parkinglot>(_serivce.GetAll<Parkinglot>())); }
            set { _parkinglots = value; }
        }

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees ?? (_employees = new ObservableCollection<Employee>(_serivce.GetAll<Employee>())); }
            set { _employees = value; }
        }

        private ObservableCollection<Region> _regions;
        public ObservableCollection<Region> Regions
        {
            get { return _regions ?? (_regions = new ObservableCollection<Region>(_serivce.GetAll<Region>())); }
            set { _regions = value; }
        }

        public GlobalViewModel(IRepository repository)
        {
            _serivce = new DataService(repository);
        }
    }
}
