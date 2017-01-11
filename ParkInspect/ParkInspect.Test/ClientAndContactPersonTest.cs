using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;
using ParkInspect.ViewModel;

namespace UnitTestProject
{
    [TestClass]
    public class ClientAndContactPersonTest
    {
        //Repo
        private MockRepositoryWrapper<Contactperson, ParkInspectEntities> _mockRepo;
        
        //Client
        private Client _testClient;
        private ClientService _clientService;

        //Contact person
        private Contactperson _testContactPerson;
        private ContactpersonService _contactPersonService;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Contactperson, ParkInspectEntities>();
            _contactPersonService = new ContactpersonService(_mockRepo.Repo);
            _clientService = new ClientService(_mockRepo.Repo);
            

            //Arrange
            _testClient = new Client
            {
                id = 1000,
                email = "TestKlant@testklant.nl",
                name = "TestKlant",
                phonenumber = "0612345678",
            };

            _testContactPerson = new Contactperson
            {
                id = 10001,
                firstname = "Sjaak",
                lastname = "Afhaak",
                Client = _testClient,
                client_id = 1000
            };
        }

        /*
            Client Tests
        */

        [TestMethod]
        [TestCategory("Client")]
        public void CreateClient()
        {
            _clientService.Add(_testClient);
            Assert.IsNotNull(_testClient);
            _clientService.Delete(_testClient);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void UpdateClient()
        {
            _clientService.Add(_testClient);
            Assert.AreNotEqual(true, _testClient);
            _clientService.Delete(_testClient);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ClientNameIsnull()
        {
            _testClient.name = null;

            var rs = _clientService.Add(_testClient);
            Assert.AreEqual(false, rs);
            _clientService.Delete(_testClient);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ClientEmailIsNull()
        {
            _testClient.email = null;

            var rs = _clientService.Add(_testClient);
            Assert.AreEqual(false, rs);
            _clientService.Delete(_testClient);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ClientPhoneNumberIsNull()
        {
            _testClient.phonenumber = null;

            var rs = _clientService.Add(_testClient);
            Assert.AreEqual(false, rs);
            _clientService.Delete(_testClient);
        }

        /*
            Contact person Tests
        */

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void CreateContactPerson()
        {
            _contactPersonService.Add(_testContactPerson);
            Assert.IsNotNull(_testContactPerson);
            _contactPersonService.Delete(_testContactPerson);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void UpdateContactPerson()
        {
            
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonFirstNameIsNull()
        {
            _testContactPerson.firstname = null;

            var rs = _contactPersonService.Add(_testContactPerson);
            Assert.AreEqual(false, rs);
            _contactPersonService.Delete(_testContactPerson);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonLastNameIsNull()
        {
            _testContactPerson.lastname = null;

            var rs = _contactPersonService.Add(_testContactPerson);
            Assert.AreEqual(false, rs);
            _contactPersonService.Delete(_testContactPerson);
        }

        [TestMethod]
        [TestCategory("ContactPerson")]
        public void ContactPersonWithoutClient()
        {
            _testContactPerson.Client = null;

            var rs = _contactPersonService.Add(_testContactPerson);
            Assert.AreEqual(false, rs);
            _contactPersonService.Delete(_testContactPerson);
        }
    }
}
