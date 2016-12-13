using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using ParkInspect.View;

namespace UnitTestProject
{
    [TestClass]
    public class LoginTests
    {
        private EntityFrameworkRepository<ParkInspectEntities> repo;
        private LoginViewModel vm;
        //private DialogCoordinator dialog;
        //private DashboardView bashboard;
        private EmployeeService service;

        [TestInitialize]
        public void init()
        {
            repo = new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities());
            //dashboard = new DashboardView();
            //vm = new LoginViewModel(dialog, repo);
            //service = new EmployeeService(repo);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Thread th = new Thread(new ThreadStart(delegate
            {
               // var window = new DashboardView();
            }));
        }
    }
}
