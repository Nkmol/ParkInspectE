using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;

namespace ParkInspect.Test
{
    [TestClass]
    public class EmployeeTests
    {
        private MockRepositoryWrapper<Employee, ParkInspectEntities> _mockRepo;
        private Employee _newEmployee;
        private EmployeeService _service;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Employee, ParkInspectEntities>();
            _service = new EmployeeService(_mockRepo.Repo);

            _newEmployee = new Employee
            {
                firstname = "Test",
                lastname = "Employee",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                active = true,
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            _service.Add(_newEmployee);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void Create()
        {
            var test = new Employee
            {
                id = 999,
                firstname = "Test",
                lastname = "Employee",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                active = true,
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            _service.Add(test);
            var employee = _service.Get(test.id);
            Assert.IsNotNull(employee);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void Update()
        {
            _newEmployee.firstname = "Test2";

            _service.Update<Employee>(_newEmployee);

            var s = _service.Get(_newEmployee.id);

            Assert.AreEqual(_newEmployee.firstname, s.firstname);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void Delete()
        {
            var test = new Employee
            {
                id = 666,
                firstname = "Test",
                lastname = "Employee",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                active = true,
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            _service.Add(test);

            var rs = _service.Delete(test);

            Assert.AreEqual(true, rs);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void GetAllEmployees()
        {
            var list = _service.GetAllEmployees().ToList();
            var list2 = _mockRepo.Repo.GetAll<Employee>().ToList();            
            CollectionAssert.AreEqual(list, list2);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void FirstNameIsNull()
        {
            var test1 = new Employee
            {
                id = 999,
                lastname = "Employee",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                active = true,
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            var rs = _service.Add(test1);
            Assert.AreEqual(false, rs);
            _service.Delete(test1);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void LastNameIsNull()
        {
            var test1 = new Employee
            {
                id = 555,
                firstname = "Employee",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                active = true,
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            var rs = _service.Add(test1);
            Assert.AreEqual(false, rs);
            _service.Delete(test1);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void ActiveIsNull()
        {
            var test1 = new Employee
            {
                id = 555,
                firstname = "Employee",
                lastname = "Test",
                password = "password",
                role = "Employee",
                employee_status = "Available",
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
                email = "r.vnp@parkinspect.nl"
            };

            var rs = _service.Add(test1);
            Assert.AreEqual(false, rs);
            _service.Delete(test1);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void InservicedateIsNull()
        {
            var test1 = new Employee
            {
                id = 555,
                firstname = "Employee",
                lastname = "Test",
                password = "password",
                active = true,
                role = "Employee",
                employee_status = "Available",
                phonenumber = "06-97834234",
                email = "r.vnp@parkinspect.nl"
            };

            var rs = _service.Add(test1);
            Assert.AreEqual(false, rs);
            _service.Delete(test1);
        }

        [TestMethod]
        [TestCategory("Employee")]
        public void EmailIsNull()
        {
            var test1 = new Employee
            {
                id = 555,
                firstname = "Employee",
                lastname = "Test",
                password = "password",
                active = true,
                role = "Employee",
                employee_status = "Available",
                phonenumber = "06-97834234",
                in_service_date = DateTime.Now,
            };

            var rs = _service.Add(test1);
            Assert.AreEqual(false, rs);
            _service.Delete(test1);
        }
    }
}
