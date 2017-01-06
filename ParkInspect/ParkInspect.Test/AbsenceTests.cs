using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;

namespace UnitTestProject
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
                employee_id =  _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(10))
            };

            _service.InsertAbsence(_newAbsence);
        }

        [TestMethod]
        public void Create()
        {
            var Absence = new Absence
            {
                employee_id = _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(20))
            };

            _service.InsertAbsence(Absence);
            var test =
                _service.GetAllAbsences()
                    .Where(a => a.employee_id == Absence.employee_id && a.end == Absence.end && Absence.start == a.start);
            var t = test.GetEnumerator().Current;
            Assert.IsNotNull(t);
        }

        [TestMethod]
        public void Update()
        {
            var Absence = new Absence
            {
                employee_id = _newEmployee.id,
                end = DateTime.Now,
                start = DateTime.Now.Subtract(new TimeSpan(20))
            };

            _service.InsertAbsence(Absence);

            Absence.start = DateTime.Now.Subtract(new TimeSpan(30));
            var test =
                _service.GetAllAbsences()
                    .Where(a => a.employee_id == Absence.employee_id && a.end == Absence.end && Absence.start == DateTime.Now.Subtract(new TimeSpan(30)));
            var t = test.GetEnumerator().Current;
            Assert.IsNotNull(t);
        }
    }
}
