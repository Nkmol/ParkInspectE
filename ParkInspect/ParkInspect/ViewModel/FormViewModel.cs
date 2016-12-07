using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using ParkInspect.Repository;


namespace ParkInspect.ViewModel
{
    public class FormViewModel : ViewModelBase
    {
        public EntityFrameworkRepository<ParkInspectEntities> Context { get; set; }

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

        public RelayCommand NewTemplateCommand { get; set; }
        public RelayCommand EditTemplateCommand { get; set; }
        public RelayCommand NewListCommand { get; set; }

        public Template selectedTemplate;
        public List<Template> templates { get; set; }
        public List<double> templateVersions { get; set; }


        public NewTemplateViewModel NewTemplateViewModel { get; set; }

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

        public void newTemplate()
        {
            // create new template
            enableEditor();
            NewTemplateViewModel.newTemplate();
        }

        public void editTemplate()
        {
            // edit from template
            enableEditor();
            //NewTemplateViewModel.setTemplate();
        }
    }
}