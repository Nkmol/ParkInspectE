﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Model.Factory;
using ParkInspect.Model.Factory.Builder;
using ParkInspect.Repository;
using ParkInspect.Services;

namespace ParkInspect.ViewModel
{
    public class ContactpersonViewModel : ViewModelBase
    {
        private string _clientFilter;

        private string _firstnameFilter;
        private string _lastnameFilter;
        private Client _selectedClient;

        private Contactperson _selectedContactperson;

        private DialogManager _dialog;

        protected ContactpersonService Service;

        public ContactpersonViewModel(IRepository context, DialogManager dialog)
        {
            _dialog = dialog;
            Service = new ContactpersonService(context);
            Data = Service.GetAll<Contactperson>();
            Clients = new ObservableCollection<Client>(Service.GetAll<Client>());
            ResetButtonCommand = new RelayCommand(Reset);
            SaveCommand = new RelayCommand(SaveContactperson);
            DeleteContactpersonCommand = new RelayCommand(DeleteContactperson, CanDelete);
            SelectedClient = new Client();
            UpdateContactpersons();
            Reset();
        }

        private IEnumerable<Contactperson> Data { get; set; }

        public ObservableCollection<Contactperson> Contactpersons { get; set; }
        public ObservableCollection<Client> Clients { get; set; }

        public Contactperson SelectedContactperson
        {
            get { return _selectedContactperson; }
            set
            {
                _selectedContactperson = value;

                if (_selectedContactperson?.Client != null)
                    SelectedClient = _selectedContactperson.Client;

                RaisePropertyChanged("firstname");
                RaisePropertyChanged("lastname");
                RaisePropertyChanged("client_id");
                DeleteContactpersonCommand.RaiseCanExecuteChanged();
            }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                Set(ref _selectedClient, value);
                RaisePropertyChanged(("SelectedClient"));
            }
        }

        public string Firstname
        {
            get { return _selectedContactperson.firstname; }
            set
            {
                _selectedContactperson.firstname = value;
                RaisePropertyChanged(("Firstname"));
            }
        }

        public string Lastname
        {
            get { return _selectedContactperson.lastname; }
            set
            {
                _selectedContactperson.lastname = value;
                RaisePropertyChanged(("Lastname"));
            }
        }

        public string FirstnameFilter
        {
            get { return _firstnameFilter; }
            set
            {
                _firstnameFilter = value;
                UpdateContactpersons();
            }
        }

        public string LastnameFilter
        {
            get { return _lastnameFilter; }
            set
            {
                _lastnameFilter = value;
                UpdateContactpersons();
            }
        }

        public string ClientFilter
        {
            get { return _clientFilter; }
            set
            {
                _clientFilter = value;
                UpdateContactpersons();
            }
        }

        public ICommand ResetButtonCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand DeleteContactpersonCommand { get; set; }

        private void Reset()
        {
            SelectedContactperson = new Contactperson();
            RaisePropertyChanged("SelectedContactperson");
            SelectedContactperson.id = -1;
        }

        private void UpdateContactpersons()
        {
            Data = Service.GetAll<Contactperson>();

            var builder = new FilterBuilder();
            builder.Add("firstname", FirstnameFilter);
            builder.Add("lastname", LastnameFilter);
            builder.Add("Client.name", ClientFilter);

            var result = Data.Where(x => x.Like(builder.Get()));

            Contactpersons = new ObservableCollection<Contactperson>(result);
            Reset();
            RaisePropertyChanged("Contactpersons");
        }

        private bool CanDelete()
        {
            return (SelectedContactperson != null) && (SelectedContactperson.id != -1);
        }

        private void SaveContactperson()
        {
            if (SelectedContactperson.id < 0)
            {
                SelectedContactperson.client_id = SelectedClient.id;
                Service.Add(SelectedContactperson);
                _dialog.ShowMessage("Actie", "Contactpersoon toegevoegd");
            }
            else
            {
                SelectedContactperson.client_id = SelectedClient.id;
                Service.Update(SelectedContactperson);
                _dialog.ShowMessage("Actie", "Contactpersoon geupdate");
            }

            UpdateContactpersons();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();
        }

        private void DeleteContactperson()
        {
            Service.Delete<Contactperson>(SelectedContactperson);          
            UpdateContactpersons();
            DeleteContactpersonCommand.RaiseCanExecuteChanged();
            _dialog.ShowMessage("Action", "Contactpersoon verwijderd");
        }
    }
}