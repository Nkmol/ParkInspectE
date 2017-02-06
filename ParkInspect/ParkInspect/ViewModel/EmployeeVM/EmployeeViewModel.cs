using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.EmployeeVM
{
    /// <summary>
    ///     This class contains properties that a View can data bind to.
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;

        private readonly Employee Data;

        public EmployeeViewModel(IRepository context, Employee data, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new EmployeeService(context);
            Data = data;

            //Initialize startup Employee
            Data.in_service_date = DateTime.Today;
            Data.out_service_date = DateTime.Today;

            SaveCommand = new RelayCommand<EmployeeOverviewViewModel>(Save);
            Reset();
        }

        //Service
        protected EmployeeService Service { get; set; }

        public bool InspectorSelected => FormRole?.role1 == "Inspecteur";

        public bool RegionSelected => (FormRole?.role1 == "Inspecteur") && (FormRegion != null);

        public bool CanSave
            =>
            (FormRole != null) &&
            (((FormRole.role1 == "Inspecteur") && !string.IsNullOrWhiteSpace(FormRegion?.name)) ||
             (FormRole.role1 != "Inspecteur"));

        public string Message { get; set; }

        //Commands
        public RelayCommand<EmployeeOverviewViewModel> SaveCommand { get; set; }

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

            overview.NewEmployee();
            overview.EmployeesChanged();
        }

        private void Add(EmployeeOverviewViewModel overview)
        {
            SaveForm();

            var error = false;

            var statusses = new Dictionary<bool, List<string>>
            {
                {
                    true, new List<string>
                    {
                        "gepensioneerd",
                        "beëindigde dienst"
                    }
                },
                {
                    false, new List<string>
                    {
                        "beschikbaar",
                        "onbetaald verlof",
                        "geschorst",
                        null
                    }
                }
            };

            var notifications = new Dictionary<bool, List<string>>
            {
                {
                    true, new List<string>
                    {
                        "Een medewerker kan niet 'actief' zijn als hij/zij met pensioen is.",
                        "Een medewerker kan niet 'actief' zijn als hij/zij ontslagen is."
                    }
                },
                {
                    false, new List<string>
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
            Message = Service.Update(Data)
                ? "Het personeelslid is aangepast!"
                : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Personeel bewerken", Message);
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
                RaisePropertyChanged("InspectorSelected");
                RaisePropertyChanged("RegionSelected");
                RaisePropertyChanged("CanSave");
            }
        }

        private Region _formRegion;

        public Region FormRegion
        {
            get { return _formRegion; }
            set
            {
                _formRegion = value;
                RaisePropertyChanged("InspectorSelected");
                RaisePropertyChanged("RegionSelected");
                RaisePropertyChanged("CanSave");
            }
        }

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
    }
}