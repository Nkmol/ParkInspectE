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
using ParkInspect.Repository;
using ParkInspect.ViewModel;
using System.Diagnostics;

namespace ParkInspect.ViewModel
{
    public class TemplatesViewModel : ViewModelBase, IPopup
    {
        private TemplateService _service;
        public TemplateService Service
        {
            get
            {
                if (_service == null && superViewModel.Context != null)
                {
                    _service = new TemplateService(superViewModel.Context);
                }
                return _service;
            }
        }

        public FormViewModel superViewModel { get; }
        private IRepository Context { get { return superViewModel.Context; } }

        private ObservableCollection<TemplateCollection> _templates;
        public ObservableCollection<TemplateCollection> Templates
        {
            get
            {
                return _templates;
            }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }

        private ObservableCollection<string> _versions;
        public ObservableCollection<string> Versions
        {
            get
            {
                return _versions;
            }
            set
            {
                _versions = value;
                RaisePropertyChanged("Versions");
                SelectedVersion = _versions.Max();
            }
        }

        private TemplateCollection _selectedTemplateCollection;
        public TemplateCollection SelectedTemplateCollection {
            get
            {
                return _selectedTemplateCollection;
            }
            set
            {
                _selectedTemplateCollection = value;
                RaisePropertyChanged("SelectedTemplateCollection");
                List<string> versions = new List<string>();
                foreach(Template t in _selectedTemplateCollection.templates)
                {
                    versions.Add(t.version_number);
                }
                Versions = new ObservableCollection<string>(versions);
            }
        }

        private string _SelectedVersion;
        public string SelectedVersion
        {
            get
            {
                return _SelectedVersion;
            }
            set
            {
                _SelectedVersion = value;
                RaisePropertyChanged("SelectedVersion");
            }
        }

        public NewTemplateViewModel NewTemplateViewModel { get; set; }

        public RelayCommand NewTemplateCommand { get; set; }
        public RelayCommand EditTemplateCommand { get; set; }
        public RelayCommand NewListCommand { get; set; }

        public object SelectedItemPopup
        {
            get
            {
                TemplateCollection collection = SelectedTemplateCollection;
                Template template = collection.getTemplateFromVersion(SelectedVersion);
                return template;
            }
        }

        public TemplatesViewModel(FormViewModel super)
        {
            superViewModel = super;
            NewTemplateCommand = new RelayCommand(newTemplate);
            EditTemplateCommand = new RelayCommand(editTemplate);

            NewTemplateViewModel = new NewTemplateViewModel(this);
            fillTemplates();
            SelectedTemplateCollection = Templates.First();
        }

        public void fillTemplates()
        {
            Templates = new ObservableCollection<TemplateCollection>(new TemplateCollection[]{ });
            IRepository Context = this.Context;
            if (!((EntityFrameworkRepository < ParkInspectEntities > )Context).IsConnected())
            {
                Context = ViewModelLocator.localRepo;
            }
            foreach(Template template in Context.GetAll<Template>())
            {
                bool added = false;
                for (int i = 0; i < Templates.Count; i++)
                {
                    TemplateCollection collection = Templates[i];
                    if (collection.canContain(template))
                    {
                        collection.addTemplate(template);
                        added = true;
                        Debug.WriteLine("{0} : {1}", collection.name, collection.highestVersion);
                        break;
                    }
                }
                if (added == false)
                {
                    TemplateCollection collection = new TemplateCollection(template.name);
                    collection.addTemplate(template);
                    Templates.Add(collection);
                }
            }
            RaisePropertyChanged("Templates");
        }

        public void newTemplate()
        {
            // create new template
            superViewModel.EnableEditor();
            NewTemplateViewModel.newTemplate();
        }

        public void editTemplate()
        {
            // edit from template
            superViewModel.EnableEditor();
            TemplateCollection collection = SelectedTemplateCollection;
            Template template = collection.getTemplateFromVersion(SelectedVersion);
            NewTemplateViewModel.SetTemplate(Service.editTemplate(template));
        }

    }

    public struct TemplateCollection
    {
        public ObservableCollection<Template> templates { get; set; }
        public string highestVersion
        {
            get
            {
                string highest = "0.0";
                foreach (Template t in templates)
                {
                    if (highest.CompareTo(t.version_number) == -1)
                    {
                        highest = t.version_number;
                    }
                }
                return highest;
            }
        }
        public string name { get; set; }

        public TemplateCollection(string name)
        {
            this.name = name;
            templates = new ObservableCollection<Template>(new Template[] { });
        }

        public void addTemplate(Template template)
        {
            templates.Add(template);
        }

        public bool canContain(Template template)
        {
            foreach(Template t in templates)
            {
                if (t.name == template.name)
                {
                    return true;
                }
            }
            return false;
        }

        public Template getTemplateFromVersion(string version)
        {
            foreach (Template t in templates)
            {
                if (version == t.version_number)
                {
                    return t;
                }
            }
            return null;
        }
    }
}
