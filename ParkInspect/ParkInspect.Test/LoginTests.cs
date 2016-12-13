using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using System.Windows;

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

            _service.Add(_newEmpolyee); // Fake employee
        }

        // Small/single asserts are good!
        // http://softwareengineering.stackexchange.com/questions/7823/is-it-ok-to-have-multiple-asserts-in-a-single-unit-test

        [TestMethod]
        public void CanLogin()
        {
            // Assert
            var result = _service.Login("henk@henk.nl", "ab123");
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void WrongPassword()
        {
            // Assert
            var result = _service.Login("henk@henk.nl", "ab12333");
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void WrongUsername()
        {
            // Assert
            var result = _service.Login("henk@henkie.nl", "ab123");
            Assert.AreEqual(result, false);
        }

        // Not of this class, just as example
        [TestMethod]
        public void TestUpdate()
        {
            // Arrange
            _newEmpolyee.firstname = "Jan";

            // Act
            _service.Update(_newEmpolyee);

            // Assert
            var employee = _service.Get(_newEmpolyee.id);
            Assert.AreEqual(employee.firstname, "Jan");
        }
    }
}
