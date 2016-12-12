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
using System.Windows;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;

namespace ParkInspect.ViewModel
{
    public class OfflineViewMode : ViewModelBase
    {
        protected InspectionService service;
        public ObservableCollection<String> directionItems { get; set; }
        public ObservableCollection<String> directions { get; set; }
        String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private string _selectedDirection;
        public String SelectedDirection
        {
            get
            {
                return _selectedDirection;
            }
            set
            {
                _selectedDirection = value;
                LoadDirectionItems();
                base.RaisePropertyChanged();

            }
        }
        public OfflineViewMode(IRepository context)
        {
            directions = new ObservableCollection<string>();
            directionItems = new ObservableCollection<string>();
        }
        private void LoadDirectionItems()
        {
            String line;
            System.IO.StreamReader file = new System.IO.StreamReader(runpath + "/directions/" + _selectedDirection + ".txt");
            while ((line = file.ReadLine()) != null)
            {
                directionItems.Add(line);

            }
        }

        public void LoadDirections()
        {
            foreach (String name in Directory.GetFiles(runpath + "/directions", "*.txt").Select((Path.GetFileNameWithoutExtension)))
            {
                directions.Add(name);
            }
        }
    }
}
