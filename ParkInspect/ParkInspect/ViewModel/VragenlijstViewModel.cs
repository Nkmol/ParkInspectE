using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;


namespace ParkInspect.ViewModel
{
    public class VragenlijstViewModel : ViewModelBase
    {
        private Visibility editorVisilibty;
        public Visibility EditorVisibility {
            get {
                return editorVisilibty;
            }
            set {
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

        public RelayCommand NewTemplateCommand { get; set; }
        public RelayCommand EditTemplateCommand { get; set; }
        public RelayCommand NewListCommand { get; set; }

        //public Template selectedTemplate;
        //public IEnumerable<Template> templates;
        //public IEnumerable<double> templateVersions{ get; set; }
        //

        public NewTemplateViewModel NewTemplateViewModel { get; set; }
        
        public void enableEditor()
        {
            EditorVisibility = Visibility.Visible;
            SelectedTab = 2;
        }

        public void disableEditor()
        {
            EditorVisibility = Visibility.Hidden;
            if (SelectedTab == 2)
            {
                selectedTab = 1;
            }
        }

        public void newTemplate()
        {
            // create new template
            enableEditor();
            NewTemplateViewModel.setTemplate();
        }

        public void editTemplate()
        {
            // edit from template
            enableEditor();
            NewTemplateViewModel.setTemplate();
        }

        public VragenlijstViewModel()
        {
            EditorVisibility = Visibility.Hidden;
            NewTemplateCommand = new RelayCommand(newTemplate);
            EditTemplateCommand = new RelayCommand(editTemplate);

            NewTemplateViewModel = new NewTemplateViewModel(this);
        }
    }
}