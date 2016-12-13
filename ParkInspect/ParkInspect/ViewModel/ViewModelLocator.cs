/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ParkInspect.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using ParkInspect.Model;
using ParkInspect.Repository;
using ParkInspect.View.UserControls.Popup;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<IRepository>(() => new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities()));
            SimpleIoc.Default.Register<EntityFrameworkRepository<ParkInspectEntities>>(() => new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities()));
            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            SimpleIoc.Default.Register<PopupCoordinator>();

            SimpleIoc.Default.Register<ClientViewModel>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<ExportViewModel>();
            SimpleIoc.Default.Register<EmployeeViewModel>();
            SimpleIoc.Default.Register<InspectionViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<FormViewModel>();
            //ServiceLocator.Current.GetInstance<FormViewModel>().Context = ServiceLocator.Current.GetInstance<EntityFrameworkRepository<ParkInspectEntities>>();
            SimpleIoc.Default.Register<ParkinglotViewModel>();
            SimpleIoc.Default.Register<ExportViewModel>();
            SimpleIoc.Default.Register<AbsenceViewModel>();

            SimpleIoc.Default.Register<PopupViewModel>();
            SimpleIoc.Default.Register<PopupManager>();
            SimpleIoc.Default.Register<DialogManager>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DashboardViewModel Dashboard => ServiceLocator.Current.GetInstance<DashboardViewModel>();

	public ExportViewModel Export => ServiceLocator.Current.GetInstance<ExportViewModel>();

        public EmployeeViewModel Employees => ServiceLocator.Current.GetInstance<EmployeeViewModel>();

        public InspectionViewModel Inspections => ServiceLocator.Current.GetInstance<InspectionViewModel>();

        public ReportViewModel Reports => ServiceLocator.Current.GetInstance<ReportViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();
        
        public FormViewModel Forms => ServiceLocator.Current.GetInstance<FormViewModel>();

        public ParkinglotViewModel Parkinglots => ServiceLocator.Current.GetInstance<ParkinglotViewModel>();

        public AbsenceViewModel Absence => ServiceLocator.Current.GetInstance<AbsenceViewModel>();

        public IRepository Context => ServiceLocator.Current.GetInstance<IRepository>();

        public ClientViewModel Client => ServiceLocator.Current.GetInstance<ClientViewModel>();

        public PopupViewModel Popup => new PopupViewModel(); // Always new link
        public PopupManager PopupManager => ServiceLocator.Current.GetInstance<PopupManager>();
        public DialogManager Dialog => ServiceLocator.Current.GetInstance<DialogManager>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}