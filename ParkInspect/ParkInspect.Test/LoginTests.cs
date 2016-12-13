using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using ParkInspect.View;
using System.Data.Entity;

namespace UnitTestProject
{
    [TestClass]
    public class LoginTests
    {
        private MockRepositoryWrapper<Employee, ParkInspectEntities> _mockRepo;
        private EmployeeService _service;
        private Employee _newEmpolyee;

        [TestInitialize]
        public void init()
        {
            _mockRepo = new MockRepositoryWrapper<Employee, ParkInspectEntities>();
            _service = new EmployeeService(_mockRepo.Repo);

            // Arrange
            _newEmpolyee = new Employee()
            {
                id = -1,
                employee_status = "Example",
                role = "role1",
                firstname = "henk",
                lastname = "de man",
                active = true,
                phonenumber = "1111111111",
                in_service_date = DateTime.Now,
                out_service_date = null,
                email = "henk@henk.nl",
                password = "ab123",
            };

            _service.Add(_newEmpolyee);
        }

        // Small/single asserts are good!
        // http://softwareengineering.stackexchange.com/questions/7823/is-it-ok-to-have-multiple-asserts-in-a-single-unit-test

        [TestMethod]
        public void UserIdExists()
        {
            // Assert
            var employee = _service.Get(_newEmpolyee.id);
            Assert.AreEqual(employee.id, _newEmpolyee.id);
        }

        [TestMethod]
        public void UserIdExists_False()
        {
            // Assert
            var employee = _service.Get(_newEmpolyee.id);
            Assert.AreNotEqual(employee.id, 2);
        }

        [TestMethod]
        public void UserEmailExists()
        {
            // Assert
            var employee = _service.Get(_newEmpolyee.id);
            Assert.AreEqual(employee.id, _newEmpolyee.id);
        }
    }
}
