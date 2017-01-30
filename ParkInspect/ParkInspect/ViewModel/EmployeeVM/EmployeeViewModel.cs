using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.EmployeeVM
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

        private readonly Employee Data;

        public bool InspectorSelected { get; set; }

        private readonly DialogManager _dialog;

        public string Message { get; set; }

        //Commands
        public RelayCommand<EmployeeOverviewViewModel> SaveCommand { get; set; }

        public EmployeeViewModel(IRepository context, Employee data, DialogManager dialog)
        {
            _dialog = dialog;
            //Service and employees
            Service = new EmployeeService(context);
            Data = data;

            //Initialize startup Employee
            Data.in_service_date = DateTime.Today;
            Data.out_service_date = DateTime.Today;

            //Initialize commands
            SaveCommand = new RelayCommand<EmployeeOverviewViewModel>(Save);
            Reset();
            FillForm();
        }

        #region Properties

        public string Email
        {
            get { return Data.email; }
            set { Data.email = value; }
        }

        public Role Role
        {
            get { return Data.Role1; }
            set { Data.Role1 = value; }
        }

        public Region Region
        {
            get { return Data.Region; }
            set { Data.Region = value; }
        }

        public Employee_Status EmployeeStatus
        {
            get { return Data.Employee_Status1; }
            set { Data.Employee_Status1 = value; }
        }

        public string Firstname
        {
            get { return Data.firstname; }
            set { Data.firstname = value; }
        }

        public string Lastname
        {
            get { return Data.lastname; }
            set { Data.lastname = value; }
        }

        public string Password
        {
            get { return Data.password; }
            set { Data.password = value; }
        }

        public string Phonenumber
        {
            get { return Data.phonenumber; }
            set { Data.phonenumber = value; }
        }

        public bool Active
        {
            get { return Data.active; }
            set { Data.active = value; }
        }

        public DateTime InServiceDate
        {
            get { return Data.in_service_date; }
            set { Data.in_service_date = value; }
        }

        public DateTime? OutServiceDate
        {
            get { return Data.out_service_date; }
            set { Data.out_service_date = value; }
        }

        public ICollection<Absence> Absences
        {
            get { return Data.Absences; }
            set { Data.Absences = value; }
        }

        public ICollection<Inspection> Inspections
        {
            get { return Data.Inspections; }
            set { Data.Inspections = value; }
        }

        #endregion

        #region Form

        public string FormEmail { get; set; }

        private Role _formRole;
        public Role FormRole
        {
            get { return _formRole; }
            set
            {
                _formRole = value;
                if (value?.role1 == "Inspector")
                {
                    InspectorSelected = true;
                    RaisePropertyChanged("InspectorSelected");
                }
            }
        }

        public Region FormRegion { get; set; }

        public Employee_Status FormEmployeeStatus { get; set; }

        public string FormFirstname { get; set; }

        public string FormLastname { get; set; }

        public string FormPassword { get; set; }

        public string FormPhonenumber { get; set; }

        public bool FormActive { get; set; }

        public DateTime FormInServiceDate { get; set; }

        public DateTime? FormOutServiceDate { get; set; }

        public ICollection<Absence> FormAbsences { get; set; }

        public ICollection<Inspection> FormInspections { get; set; }

        #endregion

        private void SaveForm()
        {
            Firstname = FormFirstname;
            Lastname = FormLastname;
            Phonenumber = FormPhonenumber;
            Email = FormEmail;
            Password = FormPassword;
            InServiceDate = FormInServiceDate;
            OutServiceDate = FormOutServiceDate;
            Active = FormActive;
            Absences = FormAbsences;
            Inspections = FormInspections;
            EmployeeStatus = FormEmployeeStatus;
            Region = FormRegion;
            Role = FormRole;
        }

        private void FillForm()
        {
            FormFirstname = Firstname;
            FormLastname = Lastname;
            FormPhonenumber = Phonenumber;
            FormEmail = Email;
            FormPassword = Password;
            FormInServiceDate = InServiceDate;
            FormOutServiceDate = OutServiceDate;
            FormActive = Active;
            FormAbsences = Absences;
            FormInspections = Inspections;
            FormEmployeeStatus = EmployeeStatus;
            FormRegion = Region;
            FormRole = Role;
        }

        public void Reset()
        {
            FillForm();
        }

        private void Save(EmployeeOverviewViewModel overview)
        {
            if (Data.id <= 0)
                Add(overview);
            else
                Edit();
        }

        private void Add(EmployeeOverviewViewModel overview)
        {
            SaveForm();

            var error = false;

            var statusses = new Dictionary<bool, List<string>>()
            {
                {
                    true, new List<string>()
                    {
                        "Retired",
                        "Terminated"
                    }
                },
                {
                    false, new List<string>()
                    {
                        "Available",
                        "On Non-Pay leave",
                        "Suspended",
                        null
                    }
                }
            };

            var notifications = new Dictionary<bool, List<string>>()
            {
                {
                    true, new List<string>()
                    {
                        "Een medewerker kan niet 'actief' zijn als hij/zij met pensioen is.",
                        "Een medewerker kan niet 'actief' zijn als hij/zij ontslagen is."
                    }
                },
                {
                    false, new List<string>()
                    {
                        "Een medewerker kan niet 'beschikbaar' zijn als hij/zij geen lopend contract heeft.",
                        "Een medewerker kan niet 'Op betaald verlof' zijn als hij/zij geen lopend contract heeft.",
                        "Een medewerker kan niet 'Geschorst' zijn als hij/zij geen lopend contract heeft.",
                        "Er moet een datum uit dienst ingevoerd worden"
                    }
                }
            };

            var notification = "";

            if (statusses[Data.active].Contains(Data.Employee_Status1.employee_status1))
            {
                var id = statusses[Data.active].IndexOf(Data.Employee_Status1.employee_status1);
                notification = notifications[Data.active][id];
                error = true;
            }

            if (error)
            {
                _dialog.ShowMessage("Personeel toevoegen", notification);
                return;
            }                

            Message = Service.Add(Data)
                    ? "Het personeelslid is toegevoegd!"
                    : "Er is iets misgegaan tijdens het toevoegen.";
                _dialog.ShowMessage("Personeel toevoegen", Message);

            overview.Employees.Add(this);
        }

        private void Edit()
        {
            SaveForm();
            Message = Service.Update(Data) ? "Het personeelslid is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Personeel bewerken", Message);
        }

        /*CRU METHODS
        private void SaveEmployee()
        {
            bool error = false;

            //Boolean: employee activity
            //List: statusses
            Dictionary<bool, List<string>> statusses = new Dictionary<bool, List<string>>()
            {
                {
                    true, new List<string>()
                    {
                        "Retired",
                        "Terminated"
                    }
                },
                {
                    false, new List<string>()
                    {
                        "Available",
                        "On Non-Pay leave",
                        "Suspended",
                        null
                    }
                }
            };

            Dictionary<bool, List<string>> notifications = new Dictionary<bool, List<string>>()
            {
                {
                    true, new List<string>()
                    {
                        "Een medewerker kan niet 'actief' zijn als hij/zij met pensioen is.",
                        "Een medewerker kan niet 'actief' zijn als hij/zij ontslagen is."
                    }
                },
                {
                    false, new List<string>()
                    {
                        "Een medewerker kan niet 'beschikbaar' zijn als hij/zij geen lopend contract heeft.",
                        "Een medewerker kan niet 'Op betaald verlof' zijn als hij/zij geen lopend contract heeft.",
                        "Een medewerker kan niet 'Geschorst' zijn als hij/zij geen lopend contract heeft.",
                        "Er moet een datum uit dienst ingevoerd worden"
                    }
                }
            };

            if (statusses[SelectedEmployee.active].Contains(SelectedEmployee.employee_status))
            {
                var id = statusses[SelectedEmployee.active].IndexOf(SelectedEmployee.employee_status);
                Notification = notifications[SelectedEmployee.active][id];
                error = true;
            }

            if (!error)
            {

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
                return;
            }

            _dialog.ShowMessage("Fout opgetreden", Notification);
        }

        */
    }
}