using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel;

namespace ParkInspect.Test
{
    [TestClass]
    public class ClientAndContactPersonTest
    {
        //Repo's
        private EntityFrameworkRepository<ParkInspectEntities> _repo;
        
        //ViewModels
        private ClientViewModel _clientViewModel;
        private ContactpersonViewModel _contactpersonViewModel;

        private DialogManager dm;

        //Client
        private Client _testClient;

        //Contact person
        private Contactperson _testContactPerson;

        [TestInitialize]
        public void Init()
        {
            _repo = new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities());

            _clientViewModel = new ClientViewModel(_repo, dm);
            _contactpersonViewModel = new ContactpersonViewModel(_repo, dm);
        }

        /*
            Client Tests
        */

        [TestMethod]
        [TestCategory("Client")]
        public void CreateClient()
        {
            
        }
        /*
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
        /*
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
        */
    }
}
