﻿using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel.ClientVM
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly DialogManager _dialog;
        private readonly Client Data;
        protected ClientService Service;

        public ClientViewModel(IRepository context, Client data, DialogManager dialog)
        {
            _dialog = dialog;
            Data = data;
            Service = new ClientService(context);
            SaveCommand = new RelayCommand<ClientOverviewViewModel>(Save);
            Reset();
            Assignments = new ObservableCollection<Asignment>(Data.Asignments);
            Contactpersons = new ObservableCollection<Contactperson>(Data.Contactpersons);

            FillForm();
        }

        public ObservableCollection<Asignment> Assignments { get; set; }
        public ObservableCollection<Contactperson> Contactpersons { get; set; }

        public string Message { get; set; }
        public bool PasswordEnabled => Data.id <= 0;

        public RelayCommand<ClientOverviewViewModel> SaveCommand { get; set; }

        private void SaveForm()
        {
            Name = FormName;
            Phonenumber = FormPhonenumber;
            Email = FormEmail;

            if(Data.id <= 0) // Can only be set once with a new client
                Password = FormPassword;
        }

        private void FillForm()
        {
            FormName = Name;
            FormEmail = Email;
            FormPassword = Password;
            FormPhonenumber = Phonenumber;
        }

        public void Reset()
        {
            FillForm();
        }

        private void Save(ClientOverviewViewModel overview)
        {
            if (Data.id <= 0)
                Add(overview);
            else
                Edit();

            overview.NewClient();
        }

        public void Add(ClientOverviewViewModel overview)
        {
            SaveForm();

            var sha = SHA256.Create();

            var bytes = new byte[Data.password.Length * sizeof(char)];
            Buffer.BlockCopy(Data.password.ToCharArray(), 0, bytes, 0, bytes.Length);

            sha.ComputeHash(bytes);

            var chars = new char[sha.Hash.Length / sizeof(char)];
            Buffer.BlockCopy(sha.Hash, 0, chars, 0, sha.Hash.Length);

            Data.password = new string(chars);

            Password = Data.password;
            FormPassword = Password;

            var rs = Service.Add(Data);

            Message = rs ? "De klant is toegevoegd!" : "Er is iets misgegaan tijdens het toevoegen.";
            _dialog.ShowMessage("Klant toevoegen", Message);

            if (rs)
                overview.Clients.Add(this);
        }

        public void Edit()
        {
            SaveForm();
            Message = Service.Update(Data) ? "De klant is aangepast!" : "Er is iets misgegaan tijdens het aanpassen.";

            _dialog.ShowMessage("Klant bewerken", Message);
        }

        #region ViewModel Poco properties      

        public string Name
        {
            get { return Data.name; }
            set
            {
                Data.name = value;
                RaisePropertyChanged();
            }
        }

        public string Phonenumber
        {
            get { return Data.phonenumber; }
            set
            {
                Data.phonenumber = value;
                RaisePropertyChanged();
            }
        }

        public string Email
        {
            get { return Data.email; }
            set
            {
                Data.email = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get { return Data.password; }
            set
            {
                Data.password = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Property Form

        private string _formName;

        public string FormName
        {
            get { return _formName; }
            set { Set(ref _formName, value); }
        }

        private string _formPhonenumber;

        public string FormPhonenumber
        {
            get { return _formPhonenumber; }
            set { Set(ref _formPhonenumber, value); }
        }

        private string _formEmail;

        public string FormEmail
        {
            get { return _formEmail; }
            set { Set(ref _formEmail, value); }
        }

        private string _formPassword;

        public string FormPassword
        {
            get
            {
                return Data.id > 0
                    ? "Wachtwoord is via de website in te zien en te veranderen."
                    : _formPassword;
            }
            set { Set(ref _formPassword, value); }
        }

        #endregion
    }
}