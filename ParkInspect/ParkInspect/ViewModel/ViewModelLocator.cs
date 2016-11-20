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
using Microsoft.Practices.ServiceLocation;
using ParkInspect.Model;
using ParkInspect.Repository;

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

            SimpleIoc.Default.Register<IRepository>(() => new EntityFrameworkRepository<ParkInspectEntities1>(new ParkInspectEntities1()));

            SimpleIoc.Default.Register<ClientViewModel>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<PersoneelViewModel>();
            SimpleIoc.Default.Register<InspectieViewModel>();
            SimpleIoc.Default.Register<RapportageViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ParkeerplaatsViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]

        public DashboardViewModel Dashboard => ServiceLocator.Current.GetInstance<DashboardViewModel>();

        public PersoneelViewModel Personeel => ServiceLocator.Current.GetInstance<PersoneelViewModel>();

        public InspectieViewModel Inspecties => ServiceLocator.Current.GetInstance<InspectieViewModel>();

        public RapportageViewModel Rapportages => ServiceLocator.Current.GetInstance<RapportageViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public ParkeerplaatsViewModel Parkeerplaatsen => ServiceLocator.Current.GetInstance<ParkeerplaatsViewModel>();

        public IRepository Context => ServiceLocator.Current.GetInstance<IRepository>();

        public ClientViewModel Client => ServiceLocator.Current.GetInstance<ClientViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}