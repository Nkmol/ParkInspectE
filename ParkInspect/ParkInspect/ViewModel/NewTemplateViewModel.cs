using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections.ObjectModel;
using ParkInspect.Services;
using System.Diagnostics;

namespace ParkInspect.ViewModel
{
    
    public class NewTemplateViewModel : ViewModelBase
    {
        private TemplateService Service {
            get {
                return superViewModel.Service;
            }
        }
        private TemplatesViewModel superViewModel { get; }

        private Template _template;
        public Template Template
        {
            get {
                return _template;
            }
            set {
                _template = value;
                RaisePropertyChanged("Template");
                SaveTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        private string _fieldLabel;
        public string FieldLabel {
            get {
                return _fieldLabel;
            }
            set{
                _fieldLabel = value;
                RaisePropertyChanged("FieldLabel");
                AddFieldCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Datatype> _fieldTypes;
        public ObservableCollection<Datatype> FieldTypes {
            get{
                return _fieldTypes;
            }
            set{
                _fieldTypes = value;
                RaisePropertyChanged("FieldTypes");
            }
        }

        private Datatype _selectedFieldType;
        public Datatype SelectedFieldType {
            get
            {
                return _selectedFieldType;
            }
            set
            {
                _selectedFieldType = value;
                RaisePropertyChanged("SelectedFieldType");
                AddFieldCommand.RaiseCanExecuteChanged();
            }
        }

        private Field _selectedField;
        public Field SelectedField
        {
            get
            {
                return _selectedField;
            }
            set
            {
                _selectedField = value;
                RaisePropertyChanged("SelectedField");
                RemoveFieldCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Field> _fields;
        public ObservableCollection<Field> Fields
        {
            get
            {
                return _fields;
            }
            set
            {
                _fields = value;
                RaisePropertyChanged("Fields");
                SaveTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddFieldCommand { get; set; }
        public RelayCommand RemoveFieldCommand { get; set; }
        public RelayCommand SaveTemplateCommand { get; set; }
        public RelayCommand NameChangedCommand { get; set; }

        public NewTemplateViewModel(TemplatesViewModel superViewModel)
        {
            AddFieldCommand = new RelayCommand(AddField, CanAddField);
            RemoveFieldCommand = new RelayCommand(RemoveField, CanRemoveField);
            SaveTemplateCommand = new RelayCommand(SaveTemplate, CanSaveTemplate);
            NameChangedCommand = new RelayCommand(NameChanged);

            this.superViewModel = superViewModel;
            FieldTypes = new ObservableCollection<Datatype>(Service.central.GetAll<Datatype>().ToList<Datatype>());
            SelectedFieldType = FieldTypes.First();
        }

        public void AddField()
        {
            foreach (Field f in Template.Fields)
            {
                if (f.title == FieldLabel)
                {
                    superViewModel.superViewModel._dialog.ShowMessage("Template editor", "Een vragenlijst kan niet 2 keer dezelfde vraag bevatten");
                    return;
                }
            }
            Field field = new Field() { title = FieldLabel, datatype = SelectedFieldType.datatype1 };
            Template.Fields.Add(field);
            Fields.Add(field);
            SaveTemplateCommand.RaiseCanExecuteChanged();
        }

        public bool CanAddField()
        {
            if (SelectedFieldType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(FieldLabel))
            {
                return false;
            }
            return true;
        }

        public void RemoveField()
        {
            Template.Fields.Remove(SelectedField);
            Fields.Remove(SelectedField);
            SaveTemplateCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged("Template.Fields");
        }

        public bool CanRemoveField()
        {
            return (SelectedField != null);
        }

        public void SetTemplate(Template template)
        {
            this.Template = template;
            Fields = new ObservableCollection<Field>(template.Fields);
        }

        public void newTemplate()
        {
            SetTemplate(Service.createTemplate());
        }

        public void NameChanged()
        {
            SaveTemplateCommand.RaiseCanExecuteChanged();
        }

        public void SaveTemplate()
        {
            Service.SaveTemplate(Template);
            superViewModel.superViewModel.disableEditor();
            superViewModel.fillTemplates();
            superViewModel.superViewModel._dialog.ShowMessage("Template editor","Je template is opgeslagen als versie " + Template.version_number);
        }

        public bool CanSaveTemplate()
        {
            if (Template == null || Template.name == "" || Template.name == null || Template.Fields.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
