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

namespace ParkInspect.ViewModel
{
    
    public class NewTemplateViewModel : ViewModelBase
    {
        private TemplateService _service;
        private TemplateService Service {
            get {
                if (_service == null && superViewModel.Context != null)
                {
                    _service = new TemplateService(superViewModel.Context, superViewModel.Context);
                }
                return _service;
            }
        }
        private VragenlijstViewModel superViewModel { get; }

        private Template _template;
        public Template Template
        {
            get {
                return _template;
            }
            set {
                _template = value;
                RaisePropertyChanged("Template");
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

        private ObservableCollection<string> _fieldTypes;
        public ObservableCollection<string> FieldTypes {
            get{
                return _fieldTypes;
            }
            set{
                _fieldTypes = value;
                RaisePropertyChanged("FieldTypes");
            }
        }

        private ObservableCollection<Field> _fields;
        public ObservableCollection<Field> Fields {
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

        private string _selectedFieldType;
        public string SelectedFieldType {
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

        public string TemplateName
        {
            get
            {
                if (Template == null)
                {
                    return null;
                }
                return Template.name;
            }
            set
            {
                Template.name = value;
                RaisePropertyChanged("TemplateName");
                SaveTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand AddFieldCommand { get; set; }
        public RelayCommand RemoveFieldCommand { get; set; }
        public RelayCommand SaveTemplateCommand { get; set; }
        public RelayCommand NameChangedCommand { get; set; }

        public NewTemplateViewModel(VragenlijstViewModel superViewModel)
        {
            AddFieldCommand = new RelayCommand(AddField, CanAddField);
            RemoveFieldCommand = new RelayCommand(RemoveField, CanRemoveField);
            SaveTemplateCommand = new RelayCommand(SaveTemplate, CanSaveTemplate);
            NameChangedCommand = new RelayCommand(NameChanged);

            this.superViewModel = superViewModel;
            FieldTypes = new ObservableCollection<string>
            {
                "Tekst","Combobox","Checkbox"
            };
            SelectedFieldType = FieldTypes.First();
        }

        public void AddField()
        {
            Template.Fields.Add(new Field() { title = FieldLabel, datatype = SelectedFieldType });
            Fields = new ObservableCollection<Field>(Template.Fields);
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
        }

        public bool CanRemoveField()
        {
            return (SelectedField != null);
        }

        public void SetTemplate(Template template)
        {
            this.Template = template;
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
            superViewModel.disableEditor();
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
