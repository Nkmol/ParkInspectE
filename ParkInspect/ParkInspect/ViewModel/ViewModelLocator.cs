﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ParkInspect.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System;
using System.Xml.Serialization.Advanced;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.ServiceLocation;
using ParkInspect.Model;
using ParkInspect.Repository;
using ParkInspect.View.UserControls.Popup;
using ParkInspect.ViewModel.AssignmentVM;
using ParkInspect.ViewModel.Popup;
using ParkInspect.ViewModel.ParkinglotVM;

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

            localRepo = new EntityFrameworkRepository<ParkInspectLocalEntities>(new ParkInspectLocalEntities());


            try
            {
                SimpleIoc.Default.Register<IRepository>(() => new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities()));
                SimpleIoc.Default.Register<EntityFrameworkRepository<ParkInspectEntities>>(() => new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities()));
            } catch (Exception)
            {
                SimpleIoc.Default.Register<IRepository>(() => new EntityFrameworkRepository<ParkInspectLocalEntities>(new ParkInspectLocalEntities()));
                SimpleIoc.Default.Register<EntityFrameworkRepository<ParkInspectLocalEntities>>(() => new EntityFrameworkRepository<ParkInspectLocalEntities>(new ParkInspectLocalEntities()));
            }
            SimpleIoc.Default.Register<IDialogCoordinator, DialogCoordinator>();
            SimpleIoc.Default.Register<PopupCoordinator>();

            SimpleIoc.Default.Register<ClientViewModel>();
            SimpleIoc.Default.Register<ContactpersonViewModel>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<ExportViewModel>();
            SimpleIoc.Default.Register<EmployeeViewModel>();
            SimpleIoc.Default.Register<InspectionViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<FormViewModel>();
            //ServiceLocator.Current.GetInstance<FormViewModel>().Context = ServiceLocator.Current.GetInstance<EntityFrameworkRepository<ParkInspectEntities>>();
            SimpleIoc.Default.Register<ParkinglotViewModel>();
            SimpleIoc.Default.Register<ParkinglotOverviewViewModel>();
            SimpleIoc.Default.Register<OfflineViewModel>();
            SimpleIoc.Default.Register<PrepareViewModel>();
            SimpleIoc.Default.Register<ExportViewModel>();
            SimpleIoc.Default.Register<AbsenceViewModel>();

            SimpleIoc.Default.Register<RegionViewModel>();
            SimpleIoc.Default.Register<PopupViewModel>();
            SimpleIoc.Default.Register<PopupManager>();
            SimpleIoc.Default.Register<DialogManager>();
            SimpleIoc.Default.Register<AssignmentOverviewViewModel>();

            SimpleIoc.Default.Register<PopupViewModel>();
            SimpleIoc.Default.Register<PopupCreateUpdateViewModel>();

        }

        public static EntityFrameworkRepository<ParkInspectLocalEntities> localRepo;
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DashboardViewModel Dashboard => ServiceLocator.Current.GetInstance<DashboardViewModel>();

	    public ExportViewModel Export => ServiceLocator.Current.GetInstance<ExportViewModel>();

        public EmployeeViewModel Employees => ServiceLocator.Current.GetInstance<EmployeeViewModel>();

        public InspectionViewModel Inspection => new InspectionViewModel(SimpleIoc.Default.GetInstance<IRepository>());
        public OfflineViewModel Offline => ServiceLocator.Current.GetInstance<OfflineViewModel>();
        public PrepareViewModel Prepare => ServiceLocator.Current.GetInstance<PrepareViewModel>();
        public InspectionViewModel Inspections => ServiceLocator.Current.GetInstance<InspectionViewModel>();

        public ReportViewModel Reports => ServiceLocator.Current.GetInstance<ReportViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();
        
        public FormViewModel Forms => ServiceLocator.Current.GetInstance<FormViewModel>();

        public ParkinglotOverviewViewModel Parkinglots => ServiceLocator.Current.GetInstance<ParkinglotOverviewViewModel>();

        public AbsenceViewModel Absence => ServiceLocator.Current.GetInstance<AbsenceViewModel>();

        public RegionViewModel Region => ServiceLocator.Current.GetInstance<RegionViewModel>();

        public IRepository Context => ServiceLocator.Current.GetInstance<IRepository>();

        public ClientViewModel Client => ServiceLocator.Current.GetInstance<ClientViewModel>();

        public ContactpersonViewModel Contactperson => ServiceLocator.Current.GetInstance<ContactpersonViewModel>();

        public PopupViewModel Popup => new PopupViewModel(); // Always new link
        public PopupViewModel PopupUpdateNew => new PopupCreateUpdateViewModel(); // Always new link
        public PopupManager PopupManager => ServiceLocator.Current.GetInstance<PopupManager>();
        public DialogManager Dialog => ServiceLocator.Current.GetInstance<DialogManager>();

        public AssignmentOverviewViewModel Assignments => ServiceLocator.Current.GetInstance<AssignmentOverviewViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}