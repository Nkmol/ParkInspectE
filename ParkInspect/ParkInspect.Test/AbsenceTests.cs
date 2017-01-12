using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect.Services;

namespace ParkInspect.Test
{
    [TestClass]
    public class AbsenceTests
    {
        private MockRepositoryWrapper<Absence, ParkInspectEntities> _mockRepo;
        private MockRepositoryWrapper<Employee, ParkInspectEntities> _employeeRepo;
        private Absence _newAbsence;
        private Employee _newEmployee;
        private AbsenceService _service;
        private EmployeeService _employeeService;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Absence, ParkInspectEntities>();
            _employeeRepo = new MockRepositoryWrapper<Employee, ParkInspectEntities>();
            _service = new AbsenceService(_mockRepo.Repo);
            _employeeService = new EmployeeService(_employeeRepo.Repo);

            _newEmployee = new Employee()
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

            _employeeService.Add(_newEmployee);

            _newAbsence = new Absence
            {
                employee_id = _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(10))
            };

            _service.InsertAbsence(_newAbsence);
        }

        [TestCategory("Absence service")]
        [TestMethod]
        public void Create()
        {
            var absence = new Absence
            {
                employee_id = _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(20))
            };

            _service.InsertAbsence(absence);

            var test =
                _service
                    .GetAllAbsences().First(a => a.employee_id == absence.employee_id && a.end == absence.end && absence.start == a.start);

            Assert.IsNotNull(test);
        }

        [TestCategory("Absence service")]
        [TestMethod]
        public void Update()
        {
            var absence = new Absence
            {
                employee_id = _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(20))
            };

            _service.InsertAbsence(absence);

            absence.start = DateTime.Now.Subtract(new TimeSpan(30));

            var test =
                _service.GetAllAbsences()
                    .First(a => a.employee_id == absence.employee_id && a.end == absence.end && absence.start == DateTime.Now.Subtract(new TimeSpan(30)));

            Assert.IsNotNull(test);
        }
    }
}
