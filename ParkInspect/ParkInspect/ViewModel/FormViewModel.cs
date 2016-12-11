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


namespace ParkInspect.ViewModel
{
    public class FormViewModel : ViewModelBase
    {
        public EntityFrameworkRepository<ParkInspectEntities> Context { get; set; }
        public TemplatesViewModel TemplatesViewModel { get; set; }
        private FormService service { get; set; }

        private FormControl _view;
        public FormControl View {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                List<Template> templates = (List<Template>)Context.GetAll<Template>();
                CachedForm form = service.createFormFromTemplate(templates.ToArray()[0]);
                loadForm(form);
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
        private CachedForm? cachedForm;


        public FormViewModel(IRepository context)
        {
            Context = (EntityFrameworkRepository<ParkInspectEntities>)context;
            service = new FormService(Context, Context);
            EditorVisibility = Visibility.Hidden;
            TemplatesViewModel = new TemplatesViewModel(this);
            SaveCommand = new RelayCommand(saveForm);
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

        public void loadForm(Form form)
        {
            View.clear();
            int i = 0;
            foreach (Formfield field in form.Formfields)
            {
                CachedFormField cachedField = new CachedFormField()
                {
                    field_title = field.field_title
                };
                View.addFormField(cachedField, i);
                i++;
            }
        }

        public void loadForm(CachedForm form)
        {
            cachedForm = form;
            View.clear();
            int i = 0;
            foreach (CachedFormField field in form.fields)
            {
                View.addFormField(field, i);
                i++;
            }
        }

        public void saveForm()
        {
            Debug.WriteLine("SAVE FORM");
            if (cachedForm == null)
            {
                return;
            }
            service.SaveForm((CachedForm)cachedForm);
        }
    }

    public struct CachedForm
    {
        public Form form;
        public List<CachedFormField> fields;
        public int template_id;
    }

    public struct CachedFormField
    {
        public string field_title;
        public CachedValue value;
        public string datatype;
    }

    public struct CachedValue{
        public bool boolvalue;
        public string stringvalue;
        public int intvalue;
        public double doublevalue;
        public string type;

        public CachedValue(string sourceValue)
        {
            boolvalue = sourceValue.Replace("[Boolean]", "") == "true";
            stringvalue = sourceValue.Replace("[String]", "");
            int.TryParse(sourceValue.Replace("[Integer]", ""), out intvalue);
            double.TryParse(sourceValue.Replace("[Double]", ""), out doublevalue);
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