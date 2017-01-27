using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using ParkInspect.Repository;
using ParkInspect.View.UserControls;
using ParkInspect.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using Microsoft.Practices.ServiceLocation;
using ParkInspect.View.UserControls.Popup;
using ParkInspect.ViewModel.Popup;

/*
using Microsoft.Practices.ServiceLocation;

    ServiceLocator.Current.GetInstance<PopupManager>().ShowPopup<SelectTemplatePopup>("Template", new SelectTemplatePopup(), // line below here //);
    ServiceLocator.Current.GetInstance<FormViewModel>().createForm(SelectedInspection);
   
    ServiceLocator.Current.GetInstance<FormViewModel>().loadForm(SelectedInspection);
*/

namespace ParkInspect.ViewModel
{
    public class FormViewModel : ViewModelBase, IPopup
    {
        public IRepository Context { get; set; }
        public TemplatesViewModel TemplatesViewModel { get; set; }
        private FormService service { get; set; }
        private Inspection inspection;
        public DialogManager _dialog;

        private FormPopup _view;
        public FormPopup View {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                List<Template> templates = (List<Template>)Context.GetAll<Template>();
                List<Inspection> inspections = (List<Inspection>)Context.GetAll<Inspection>();


                //loadForm(inspections.ToArray()[0]);

                //createForm(inspections.ToArray()[0], templates.ToArray()[0]);
            }
        }

        private Visibility editorVisilibty;
        public Visibility EditorVisibility
        {
            get
            {
                return editorVisilibty;
            }
            set
            {
                editorVisilibty = value;
                RaisePropertyChanged("EditorVisibility");
            }
        }

        private int selectedTab;
        public int SelectedTab
        {
            get
            {
                return selectedTab;
            }
            set
            {
                selectedTab = value;
                RaisePropertyChanged("SelectedTab");
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddAttachmentCommand { get; set; }
        
        private CachedForm _cachedForm;
        public CachedForm cachedForm
        {
            get
            {
                return _cachedForm;
            }
            set
            {
                _cachedForm = value;
                RaisePropertyChanged("CachedForm");
            }
        }

        public object SelectedItemPopup => this;

        public FormViewModel(IRepository context, DialogManager dialog)
        {
            Context = (EntityFrameworkRepository<ParkInspectEntities>)context;
            service = new FormService(Context);
            EditorVisibility = Visibility.Hidden;
            TemplatesViewModel = new TemplatesViewModel(this);
            SaveCommand = new RelayCommand(saveForm);
            AddAttachmentCommand = new RelayCommand(addAttachment);
            _dialog = dialog;
            selectedTab = 1;
        }

        public void enableEditor()
        {
            EditorVisibility = Visibility.Visible;
            SelectedTab = 2;
        }

        public void disableEditor()
        {
            if (SelectedTab == 2)
            {
                SelectedTab = 1;
            }
            EditorVisibility = Visibility.Hidden;
        }

        public void loadForm(Inspection inspection)
        {
            this.inspection = inspection;
            Form form = inspection.Form;
            if (form == null)
            {
                return;
            }
            View.clear();
            int i = 0;
            cachedForm = new CachedForm();
            foreach (Formfield field in form.Formfields)
            {
                CachedFormField cachedField = new CachedFormField()
                {
                    field_title = field.field_title,
                    datatype = field.Field.datatype,
                    value = new CachedValue(field.value)
                };
                cachedForm.fields.Add(cachedField);
                View.addFormField(cachedField, i, true);
                i++;
            }
            selectedTab = 0;
        }

        public void createForm(Inspection inspection,Template template)
        {
            this.inspection = inspection;
            Form form = new Form();
            form.Template = template;
            CachedForm cachedForm = service.createFormFromTemplate(template);
            loadForm(cachedForm);
            SelectedTab = 0;
        }

        public void createForm(Inspection inspection)
        {

            TemplateCollection collection = TemplatesViewModel.SelectedTemplateCollection;
            Template template = collection.getTemplateFromVersion(TemplatesViewModel.SelectedVersion);
            if (template == null)
            {
                return;
            }
            createForm(inspection, template);
        }

        public void loadForm(CachedForm form)
        {
            cachedForm = form;
            View.clear();
            int i = 0;
            foreach (CachedFormField field in form.fields)
            {
                View.addFormField(field, i, false);
                i++;
            }
            selectedTab = 0;
        }

        public void saveForm()
        {
            Debug.WriteLine("SAVE FORM");
            if (cachedForm == null)
            {
                return;
            }
            service.SaveForm(inspection,cachedForm);
            _dialog.ShowMessage("Vragenlijst", "Je vragenlijst is opgeslagen.");
        }

        public void addAttachment()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".*"; // Required file extension 
            fileDialog.Filter = "Any file (.*)|*.*"; // Optional file extensions

            var result = fileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            

            string path = fileDialog.FileName;
            string content = File.ReadAllText(path);
            _cachedForm.attachments.Add(content);
            Debug.WriteLine(content);
            _dialog.ShowMessage("Vragenlijst", "Je bijlage is toegevoegd.");

        }
    }

    public class CachedForm
    {
        public Form form;
        public List<CachedFormField> fields { get; set; }
        public List<string> attachments { get; set; }
        public int template_id;

        public CachedForm()
        {
            fields = new List<CachedFormField>();
            attachments = new List<string>();
        }
    }

    public class CachedFormField
    {
        public string field_title;
        public CachedValue value { get; set; }
        public string datatype;
    }

    public class CachedValue{
        public bool boolvalue { get; set; }
        public string stringvalue { get; set; }
        public int intvalue { get; set; }
        public double doublevalue { get; set; }
        public string type { get; set; }

        public CachedValue(string sourceValue)
        {
            boolvalue = sourceValue.Replace("[Boolean]", "") == "true";
            stringvalue = sourceValue.Replace("[String]", "");

            int intvalue;
            double doublevalue;

            int.TryParse(sourceValue.Replace("[Integer]", ""), out intvalue);
            double.TryParse(sourceValue.Replace("[Double]", ""), out doublevalue);

            this.intvalue = intvalue;
            this.doublevalue = doublevalue;

            if (sourceValue.IndexOf("[Boolean]") >= 0)
            {
                type = "Boolean";
            } else if (sourceValue.IndexOf("[String]") >= 0)
            {
                type = "String";
            }
            else if (sourceValue.IndexOf("[Integer]") >= 0)
            {
                type = "Integer";
            }
            else if (sourceValue.IndexOf("[Double]") >= 0)
            {
                type = "Double";
            } else {
                type = "unknown";
            }
        }


        public override string ToString()
        {
            switch (type)
            {
                case "Boolean":
                    return "[" + type + "]" + boolvalue.ToString();
                case "String":
                    return "[" + type + "]" + stringvalue;
                case "Integer":
                    return "[" + type + "]" + intvalue.ToString();
                case "Double":
                    return "[" + type + "]" + doublevalue.ToString();
            }
            return "[string]" + stringvalue;
        }
    }
}