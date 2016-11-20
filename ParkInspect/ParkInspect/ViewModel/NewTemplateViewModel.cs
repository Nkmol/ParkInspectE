using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections.ObjectModel;

namespace ParkInspect.ViewModel
{
    
    public class NewTemplateViewModel : ViewModelBase
    {
        private VragenlijstViewModel superViewModel;

        private Template template;
        public Template Template
        {
            get {
                return template;
            }
            set {
                template = value;
                RaisePropertyChanged("Template");
            }
        }

        private string fieldLabel;
        public string FieldLabel {
            get {
                return fieldLabel;
            }
            set{
                fieldLabel = value;
                RaisePropertyChanged("FieldLabel");
                AddFieldCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<string> fieldTypes;
        public ObservableCollection<string> FieldTypes {
            get{
                return fieldTypes;
            }
            set{
                fieldTypes = value;
                RaisePropertyChanged("FieldTypes");
            }
        }

        private ObservableCollection<Field> fields;
        public ObservableCollection<Field> Fields {
            get
            {
                return fields;
            }
            set
            {
                fields = value;
                RaisePropertyChanged("Fields");
                SaveTemplateCommand.RaiseCanExecuteChanged();
            }
        }

        private string selectedFieldType;
        public string SelectedFieldType {
            get
            {
                return selectedFieldType;
            }
            set
            {
                selectedFieldType = value;
                RaisePropertyChanged("SelectedFieldType");
                AddFieldCommand.RaiseCanExecuteChanged();
            }
        }

        private Field selectedField;
        public Field SelectedField
        {
            get
            {
                return selectedField;
            }
            set
            {
                selectedField = value;
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
            AddFieldCommand = new RelayCommand(addField, canAddField);
            RemoveFieldCommand = new RelayCommand(removeField, canRemoveField);
            SaveTemplateCommand = new RelayCommand(saveTemplate, canSaveTemplate);
            NameChangedCommand = new RelayCommand(nameChanged);

            this.superViewModel = superViewModel;
            FieldTypes = new ObservableCollection<string>
            {
                "Tekst","Combobox","Checkbox"
            };
            SelectedFieldType = FieldTypes.First();
        }

        public void addField()
        {
            Template.Fields.Add(new Field() { title = FieldLabel, datatype = SelectedFieldType });
            Fields = new ObservableCollection<Field>(template.Fields);
        }

        public bool canAddField()
        {
            if (SelectedFieldType == null)
            {
                return false;
            }
            if (FieldLabel == "" || FieldLabel == null)
            {
                return false;
            }
            return true;
        }

        public void removeField()
        {
            template.Fields.Remove(selectedField);
            Fields.Remove(selectedField);
            SaveTemplateCommand.RaiseCanExecuteChanged();
        }

        public bool canRemoveField()
        {
            if (SelectedField == null)
            {
                return false;
            }
            return true;
        }

        public void setTemplate(Template template)
        {
            this.Template = template;
        }

        public void nameChanged()
        {
            SaveTemplateCommand.RaiseCanExecuteChanged();
        }

        public void saveTemplate()
        {
            superViewModel.disableEditor();
        }

        public bool canSaveTemplate()
        {
            if (template == null || template.name == "" || template.name == null || template.Fields.Count == 0)
            {
                return false;
            }
            return true;
        }
    }
}
