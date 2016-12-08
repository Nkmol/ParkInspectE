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
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;

namespace ParkInspect.ViewModel
{
    public class AbsenceViewModel : ViewModelBase
    {

        private IEnumerable<Absence> Data { get; set; }
        private ObservableCollection<Absence> _absences;
        public ObservableCollection<Absence> Absences
        {
            get
            {
                return _absences;
            }
            set
            {
                _absences = value;
                base.RaisePropertyChanged();
            }
        }

        public ObservableCollection<Employee> Employees { get; set; }

        protected AbsenceService Service;

        public ICommand SaveNewAbsenceCommand { get; set; }
        public ICommand DeleteAbsenceCommand { get; set; }


        private string _startDateFilter;
        public string StartDateFilter
        {
            get { return _startDateFilter; }
            set
            {
                _startDateFilter = value;
                UpdateAbsence();
            }
        }

        private string _endDateFilter;
        public string EndDateFilter
        {
            get { return _endDateFilter; }
            set
            {
                _endDateFilter = value;
                UpdateAbsence();
            }
        }

        private string _surnameFilter;
        public string SurnameFilter
        {
            get { return _surnameFilter; }
            set
            {
                _surnameFilter = value;
                UpdateAbsence();
            }
        }

        private string _lastNameFilter;
        public string LastNameFilter
        {
            get { return _lastNameFilter; }
            set
            {
                _lastNameFilter = value;
                UpdateAbsence();
            }
        }

        private string _idFilter;
        public string IDFilter
        {
            get { return _idFilter; }
            set
            {
                _idFilter = value;
                UpdateAbsence();
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

        private Absence _selectedAbsence;

        public Absence SelectedAbsence
        {
            get { return _selectedAbsence; }

            set
            {
                _selectedAbsence = value;
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

        private Absence _newAbsence;

        public Absence NewAbsence
        {
            get
            {
                return _newAbsence;
            }
            set
            {
                _newAbsence = value;
                base.RaisePropertyChanged();
            }
        }


        public AbsenceViewModel(IRepository context)
        {
            Service = new AbsenceService(context);
            Data = Service.GetAllAbsences();
            // look at asignment feature for datetime examples.
            Reset(); // Create default absence
            NewAbsence.start = DateTime.Now;

            Absences = new ObservableCollection<Absence>(Service.GetAllAbsences());
            Employees = new ObservableCollection<Employee>(Service.GetAllEmployees());

            SaveNewAbsenceCommand = new RelayCommand(SaveNewAbsenceMethod);
            DeleteAbsenceCommand = new RelayCommand(DeleteAbsenceMethod);
        }

        private void DeleteAbsenceMethod()
        {
            if (SelectedAbsence == null)
            {
                Notification = "Selecteer een afwezigheid";
                return;
            }

            Service.DeleteAbsence(SelectedAbsence);
            Absences.Remove(SelectedAbsence);

            base.RaisePropertyChanged();
        }

        private void SaveNewAbsenceMethod()
        {
            if (SelectedEmployee == null)
            {
                Notification = "Selecteer een werknemer.";
                return;
            }

            if (NewAbsence.start == null || NewAbsence.end == null)
            {
                Notification = "Onjuiste gegevens.";
                return;
            }
            Service.InsertAbsence(NewAbsence);
            Notification = "Nieuwe afwezigheid is opgeslagen!";
            Absences.Add(NewAbsence);
            Reset();
            base.RaisePropertyChanged();
        }

        public void Reset()
        {
            NewAbsence = new Absence
            {
                start = DateTime.Now,
                end = DateTime.Now
            };
        }

        public void UpdateAbsence()
        {
            var builder = new FilterBuilder();
            builder.Add("Employee.firstname", SurnameFilter);
            builder.Add("Employee.lastname", LastNameFilter);
            builder.Add("employee_id", IDFilter);
            builder.Add("start", StartDateFilter);
            builder.Add("end", EndDateFilter);

            var filters = builder.Get();
            var result = Data.Where(a => a.Like(filters));
            Absences = new ObservableCollection<Absence>(result);
            RaisePropertyChanged("Absences");
        }

    }
}
