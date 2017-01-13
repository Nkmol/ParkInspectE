using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;

namespace UnitTestProject
{
    [TestClass]
    public class ContactPersonTest
    {
        private MockRepositoryWrapper<Contactperson, ParkInspectEntities> _mockRepo;
        private Contactperson _testContactPerson;
        private ContactpersonService _contactPersonService;

        private Client _testClient;
        private ClientService _clientService;


        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Contactperson, ParkInspectEntities>();
            _contactPersonService = new ContactpersonService(_mockRepo.Repo);
            _clientService = new ClientService(_mockRepo.Repo);

            //Arrange
            _testClient = new Client()
            {
                id = -1,
                email = "testClient@test.nl",
                name = "Sjaak Testhaak",
                phonenumber = "0612345678"
            };

            _clientService.Add(_testClient);

            _testContactPerson = new Contactperson();

            _testContactPerson.id = -1;
            _testContactPerson.Client = _testClient;
            _testContactPerson.client_id = _testClient.id;
            _testContactPerson.firstname = "Test";
            _testContactPerson.lastname = "contacperson";


            _contactPersonService.Add(_testClient);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void CreateContactPerson()
        {
            //Arrange
            var c = new Client()
            {
                id = -1,
                email = "testClient@test.nl",
                name = "Sjaak Testhaak",
                phonenumber = "0612345678"
            };

            Contactperson cp = new Contactperson();
            cp.id = -1;
            cp.Client = c;
            cp.client_id = c.id;
            cp.firstname = "Test";
            cp.lastname = "contacperson";

            //Act
            _contactPersonService.Add(cp);

            //Assert
            var contactPerson = _contactPersonService.Get(cp);
            Assert.AreEqual("Test", cp.firstname);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void UpdateContactPerson()
        {
            //Arrange
            var c = new Client()
            {
                id = -1,
                email = "testClient@test.nl",
                name = "Sjaak Testhaak",
                phonenumber = "0612345678"
            };

            Contactperson cp = new Contactperson();
            cp.id = -1;
            cp.Client = c;
            cp.client_id = c.id;
            cp.firstname = "Test";
            cp.lastname = "contacperson";

            _contactPersonService.Add(cp);

            cp.firstname = "TestUpdate";

            //Act
            _contactPersonService.Update(cp);

            //Assert
            var res = _contactPersonService.Get(cp);
            Assert.AreEqual("TestUpdate", res.firstname);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ReadContactPerson()
        {
            //Arrange
            var c = new Client()
            {
                id = -1,
                email = "testClient@test.nl",
                name = "Sjaak Testhaak",
                phonenumber = "0612345678"
            };

            Contactperson cp = new Contactperson();
            cp.id = -1;
            cp.Client = c;
            cp.client_id = c.id;
            cp.firstname = "Test";
            cp.lastname = "contacperson";

            _contactPersonService.Add(cp);

            var res = _contactPersonService.Get(cp);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonFirstNameIsNull()
        {
            _testContactPerson.firstname = null;

            try
            {
                _contactPersonService.Add(_testContactPerson);
            }
            catch (Exception)
            {
                Assert.IsNull(true, _testContactPerson.firstname);
                throw;
            }
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonLastNameIsNull()
        {
            _testContactPerson.lastname = null;

            try
            {
                _contactPersonService.Add(_testContactPerson);
            }
            catch (Exception)
            {
                Assert.IsNull(true, _testContactPerson.lastname);
                throw;
            }
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonWithoutClient()
        {
            _testContactPerson.Client = null;

            try
            {
                _contactPersonService.Add(_testContactPerson);
            }
            catch (Exception)
            {
                Assert.IsNull(true, ""+_testContactPerson.client_id);
                throw;
            }
        }
    }
}
