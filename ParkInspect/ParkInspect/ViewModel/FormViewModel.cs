using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.View.UserControls.Popup;

namespace ParkInspect.ViewModel
{
    public class FormViewModel : ViewModelBase, IPopup
    {
        private const string FileFilter = "*.jpg;*.jpeg;*.png;";

        public static readonly string FileImagesPath = $@"{Environment.CurrentDirectory}\Assets\FormPhotos";
        private CachedForm _cachedForm;

        private Visibility _editorVisilibty;
        private Inspection _inspection;

        private int _selectedTab;

        private FormPopup _view;
        public DialogManager Dialog;

        public ObservableCollection<string> ImagePaths { get; set; }

        public IRepository Context { get; set; }
        public TemplatesViewModel TemplatesViewModel { get; set; }
        private FormService Service { get; }

        public FormPopup View
        {
            get { return _view; }
            set
            {
                _view = value;
                var templates = (List<Template>)Context.GetAll<Template>();
                var inspections = (List<Inspection>)Context.GetAll<Inspection>();
            }
        }

        public Visibility EditorVisibility
        {
            get { return _editorVisilibty; }
            set
            {
                _editorVisilibty = value;
                RaisePropertyChanged("EditorVisibility");
            }
        }

        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                RaisePropertyChanged("SelectedTab");
            }
        }

        public RelayCommand SaveCommand { get; set; }
        public RelayCommand AddAttachmentCommand { get; set; }

        public CachedForm CachedForm
        {
            get { return _cachedForm; }
            set
            {
                _cachedForm = value;
                RaisePropertyChanged("CachedForm");
            }
        }

        public object SelectedItemPopup => this;

        public RelayCommand<string> RemoveImageCommand { get; set; }

        public FormViewModel(IRepository context, DialogManager dialog)
        {
            Context = context;
            Service = new FormService(Context);
            EditorVisibility = Visibility.Hidden;
            TemplatesViewModel = new TemplatesViewModel(this);
            SaveCommand = new RelayCommand(SaveForm);
            AddAttachmentCommand = new RelayCommand(AddAttachment);
            Dialog = dialog;
            _selectedTab = 1;

            RemoveImageCommand = new RelayCommand<string>(RemoveImage);
            ImagePaths = new ObservableCollection<string>();
        }

        private void RemoveImage(string imagePath)
        {
            if (Dialog.ShowConfirmationDialog("Foto verwijderen",
                "Weet je zeker dat je de foto wilt verwijdern van de vragenlijst?"))
            {
                if (File.Exists(imagePath))
                {
                    File.SetAttributes(imagePath, FileAttributes.Normal);
                    File.Delete(imagePath);
                    ImagePaths.Remove(imagePath);
                }
            }
        }

      public void EnableEditor()
        {
            EditorVisibility = Visibility.Visible;
            SelectedTab = 2;
        }

        public void DisableEditor()
        {
            if (SelectedTab == 2)
                SelectedTab = 1;
            EditorVisibility = Visibility.Hidden;
        }

        public void LoadForm(Inspection inspection)
        {
            _inspection = inspection;
            var form = inspection.Form;
            if (form == null)
                return;
            View.clear();
            var i = 0;
            CachedForm = new CachedForm();
            foreach (var field in form.Formfields)
            {
                var cachedField = new CachedFormField
                {
                    FieldTitle = field.field_title,
                    Datatype = field.Field.datatype,
                    Value = new CachedValue(field.value)
                };
                CachedForm.Fields.Add(cachedField);
                View.addFormField(cachedField, i, false);
                i++;
            }

            var path = $@"{FileImagesPath}\{inspection.Form.id}";
            if (Directory.Exists(path))
            {
                ImagePaths = new ObservableCollection<string>(Directory.GetFiles(path));
                RaisePropertyChanged("ImagePaths");
            }
            //selectedTab = 0;
        }

        public void CreateForm(Inspection inspection, Template template)
        {
            _inspection = inspection;
            var form = new Form();
            form.Template = template;
            var cachedForm = Service.createFormFromTemplate(template);
            LoadForm(cachedForm);
            //SelectedTab = 0;
        }

        public void CreateForm(Inspection inspection)
        {
            var collection = TemplatesViewModel.SelectedTemplateCollection;
            var template = collection.getTemplateFromVersion(TemplatesViewModel.SelectedVersion);
            if (template == null)
                return;
            CreateForm(inspection, template);
        }

        public void LoadForm(CachedForm form)
        {
            if (View == null)
                new FormPopup();
            CachedForm = form;
            View.clear();
            var i = 0;
            foreach (var field in form.Fields)
            {
                View.addFormField(field, i, false);
                i++;
            }
            //selectedTab = 0;
        }

        public void SaveForm(bool isNew = false)
        {
            if (CachedForm == null)
                return;

            Service.SaveForm(_inspection, CachedForm, isNew);
            Dialog.ShowMessage("Vragenlijst", "Je vragenlijst is opgeslagen.");
        }

        public void SaveForm()
        {
            SaveForm(false);
        }

        public void AddAttachment()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".png"; // Required file extension 
            fileDialog.Filter = $@"Image Files| {FileFilter}"; // Optional file extensions

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = $@"{FileImagesPath}\{_inspection.Form.id}";
                Directory.CreateDirectory(path);
                File.Copy(fileDialog.FileName, $@"{path}\{fileDialog.SafeFileName}", true);

                ImagePaths.Add($@"{path}\{fileDialog.SafeFileName}");
                Dialog.ShowMessage("Vragenlijst foto bijlage", "Je bijlage is toegevoegd.");
            }
            else
            {
                Dialog.ShowMessage("Vragenlijst foto bijlage",
                    "Er is iets misgegaan tijdens het toevoegen van de bijlage.");
            }
        }
    }


    public class CachedForm
    {
        public Form Form;
        public int TemplateId;

        public CachedForm()
        {
            Fields = new List<CachedFormField>();
            Attachments = new List<string>();
        }

        public List<CachedFormField> Fields { get; set; }
        public List<string> Attachments { get; set; }
    }

    public class CachedFormField
    {
        public string Datatype;
        public string FieldTitle;
        public CachedValue Value { get; set; }
    }

    public class CachedValue
    {
        public CachedValue(string sourceValue)
        {
            Boolvalue = sourceValue.Replace("[Boolean]", "") == "True";
            Stringvalue = sourceValue.Replace("[String]", "");

            int intvalue;
            double doublevalue;

            int.TryParse(sourceValue.Replace("[Integer]", ""), out intvalue);
            double.TryParse(sourceValue.Replace("[Double]", ""), out doublevalue);

            Intvalue = intvalue;
            Doublevalue = doublevalue;

            if (sourceValue.IndexOf("[Boolean]") >= 0)
                Type = "Boolean";
            else if (sourceValue.IndexOf("[String]") >= 0)
                Type = "String";
            else if (sourceValue.IndexOf("[Integer]") >= 0)
                Type = "Integer";
            else if (sourceValue.IndexOf("[Double]") >= 0)
                Type = "Double";
            else Type = "unknown";
        }

        public bool Boolvalue { get; set; }
        public string Stringvalue { get; set; }
        public int Intvalue { get; set; }
        public double Doublevalue { get; set; }
        public string Type { get; set; }


        public override string ToString()
        {
            switch (Type)
            {
                case "Boolean":
                    return "[" + Type + "]" + Boolvalue;
                case "String":
                    return "[" + Type + "]" + Stringvalue;
                case "Integer":
                    return "[" + Type + "]" + Intvalue;
                case "Double":
                    return "[" + Type + "]" + Doublevalue;
            }
            return "[string]" + Stringvalue;
        }
    }
}