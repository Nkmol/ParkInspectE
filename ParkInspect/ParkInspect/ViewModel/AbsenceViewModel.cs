using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace ParkInspect.ViewModel
{
    public class AbsenceViewModel : ViewModelBase
    {
        public ObservableCollection<Absence> Absences { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }
       
        protected AbsenceService Service;

        public ICommand SaveNewAbsenceCommand { get; set; }
        public ICommand UpdateAbsenceCommand { get; set; }

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

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }

            set
            {
                Set(ref _selectedEmployee, value);

                NewAbsence.Employee = value;
                NewAbsence.employee_id = value.id;

                base.RaisePropertyChanged("SelectedEmployee");
            }
        }

        private DateTime _selectedStartTime;
        public DateTime SelectedStartTime
        {
            get { return _selectedStartTime; }

            set
            {
                Set(ref _selectedStartTime, value);

                base.RaisePropertyChanged("SelectedStartTime");
            }
        }

        private Absence _absence;

        public Absence NewAbsence
        {
            get
            {
                return _absence;
            }
            set
            {
                _absence = value;
                base.RaisePropertyChanged();
            }
        }


        public AbsenceViewModel(IRepository context)
        {
            Service = new AbsenceService(context);
            // look at asignment feature for datetime examples.
            NewAbsence = new Absence();
            NewAbsence.start = DateTime.Now;
           
            Absences = new ObservableCollection<Absence>(Service.GetAllAbsences());
            Employees = new ObservableCollection<Employee>(Service.GetAllEmployees());

            SaveNewAbsenceCommand = new RelayCommand(SaveNewAbsenceMethod);
            UpdateAbsenceCommand = new RelayCommand(UpdateAbsenceMethod);
        }

        private void SaveNewAbsenceMethod()
        {
            if (SelectedEmployee == null)
            return;
            if (NewAbsence == null) return;

            // validation
            Service.InsertAbsence(NewAbsence);
            
            Notification = "Nieuwe afwezigheid is opgeslagen!";
        }

        private void UpdateAbsenceMethod()
        {
            Notification = "Afwezigheid is geupdate!";
        }
    }
}
