using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.AbsenceVM
{
    public class AbsenceViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;

        protected AbsenceService Service;

        public AbsenceViewModel(IRepository context, Absence data, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new AbsenceService(context);
            Data = data;
            SaveCommand = new RelayCommand<AbsenceOverviewViewModel>(Save);
            DeleteCommand = new RelayCommand<AbsenceOverviewViewModel>(Delete);
            Reset();
        }

        public Absence Data { get; }

        public RelayCommand<AbsenceOverviewViewModel> SaveCommand { get; set; }
        public RelayCommand<AbsenceOverviewViewModel> DeleteCommand { get; set; }

        public string Message { get; set; }
        public bool CanDelete => Data.employee_id > 0;

        public void Reset()
        {
            FillForm();
        }

        private void SaveForm()
        {
            Start = FormStart;
            End = FormEnd;
            Employee = FormEmployee;
        }

        private void FillForm()
        {
            FormStart = Start;
            if (End != null) FormEnd = (DateTime) End;
            FormEmployee = Employee;
        }

        private void Save(AbsenceOverviewViewModel overview)
        {
            if (Data.employee_id <= 0)
                Add(overview);
            else
                Edit();

            overview.NewAbsence();
        }

        private void Add(AbsenceOverviewViewModel overview)
        {
            SaveForm();

            if (Data.start >= Data.end)
            {
                _dialog.ShowMessage("Er ging iets fout!", "De einddatum  mag niet voor de begindatum liggen!");
                return;
            }

            var rs = Service.Add(Data);

            Message = rs
                ? "De absentie is toegevoegd!"
                : "Er is iets misgegaan tijdens het toevoegen.";
            _dialog.ShowMessage("Absentie toevoegen", Message);

            if (rs)
                overview.Absences.Add(this);
        }

        private void Edit()
        {
            SaveForm();

            Message = Service.Update(Data)
                ? "De absentie is aangepast!"
                : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Absentie bewerken", Message);
        }

        private void Delete(AbsenceOverviewViewModel overview)
        {
            var message = Service.Delete(Data)
                ? "De Absentie is verwijderd!"
                : "Er is iets misgegaan tijdens het verwijderen.";

            _dialog.ShowMessage("Absentie verwijderen", message);

            overview.Absences.Remove(this);
            overview.NewAbsence();
        }

        #region Properties

        public DateTime Start
        {
            get { return Data.start; }
            set { Data.start = value; }
        }

        public DateTime? End
        {
            get { return Data.end; }
            set { Data.end = value; }
        }

        public Employee Employee
        {
            get { return Data.Employee; }
            set { Data.Employee = value; }
        }

        #endregion

        #region Property Form

        private DateTime _formStart;

        public DateTime FormStart
        {
            get { return _formStart; }
            set { Set(ref _formStart, value); }
        }

        private DateTime _formEnd;

        public DateTime FormEnd
        {
            get { return _formEnd; }
            set { Set(ref _formEnd, value); }
        }

        private Employee _formEmployee;

        public Employee FormEmployee
        {
            get { return _formEmployee; }
            set { Set(ref _formEmployee, value); }
        }

        #endregion

    }
}