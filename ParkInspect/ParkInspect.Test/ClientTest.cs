using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using UnitTestProject;

namespace ParkInspect.Test
{
    [TestClass]
    public class ClientTest
    {
        private MockRepositoryWrapper<Client, ParkInspectEntities> _mockRepo;
        private Client _testClient;
        private ClientService _service;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Client, ParkInspectEntities>();
            _service = new ClientService(_mockRepo.Repo);

            //Arrange
            _testClient = new Client
            {
                id = -1,
                email = "testClient@test.nl",
                name = "Sjaak Testhaak",
                phonenumber = "0612345678"
            };

            _service.Add(_testClient);
        }

        /*
            Client Tests
        */

        [TestMethod]
        [TestCategory("Client")]
        public void CreateClient()
        {
            //Arrange
            var tc = new Client()
            {
                id = -1,
                email = "TestCreate@test.nl",
                name = "Sjaak CreateHaak",
                phonenumber = "0612345678"
            };

            //Act
            _service.Add(tc);

            //Assert
            var client = _service.Get(tc);
            Assert.AreEqual("Sjaak CreateHaak", tc.name);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void UpdateClient()
        {
            //Arrange
            _testClient.name = "Update Client";

            //Act
            _service.Update(_testClient);

            //Assert
            var client = _service.Get(_testClient);
            Assert.AreEqual("Update Client", client.name);
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ReadClient()
        {
            var client = _service.Get(_testClient);

            Assert.IsNotNull(client);
        }


        [TestMethod]
        [TestCategory("Client")]
        public void ClientNameIsnull()
        {
            _testClient.name = null;

            try
            {
                _service.Add(_testClient);
            }
            catch (Exception)
            {
                Assert.IsNull(true, _testClient.name);
                throw;
            }
        }
        
        [TestMethod]
        [TestCategory("Client")]
        public void ClientEmailIsNull()
        {
            _testClient.email = null;

            try
            {
                _service.Add(_testClient);
            }
            catch (Exception)
            {
                Assert.IsNull(true, _testClient.email);
                throw;
            }
        }
        
        [TestMethod]
        [TestCategory("Client")]
        public void ClientPhoneNumberIsNull()
        {
            _testClient.phonenumber = null;

            try
            {
                _service.Add(_testClient);
            }
            catch (Exception)
            {
                Assert.IsNull(true, _testClient.phonenumber);
                throw;
            }
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ClientWrongPhoneNumber()
        {
            //Arrange
            string[] wrongNumbers = new string[3];

            wrongNumbers[0] = "112";
            wrongNumbers[1] = "061234567";
            wrongNumbers[2] = "06-123456";

            for (int i = 0; i < wrongNumbers.Length; i++)
            {
                var check = Regex.IsMatch(Convert.ToString(wrongNumbers[i]).Trim(),
                @"(^\+[0-9]{2}|^\+[0-9]{2}\(0\)|^\(\+[0-9]{2}\)\(0\)|^00[0-9]{2}|^0)([0-9]{9}$|[0-9\-\s]{10}$)");

                Assert.AreEqual(false, check);
            }
        }

        [TestMethod]
        [TestCategory("Client")]
        public void ClientWrongEmailAdress()
        {
            string[] wrongEmails = new string[3];

            wrongEmails[0] = "Sjaak@sjaak";
            wrongEmails[1] = "Sjaak.nl";
            wrongEmails[2] = "sjaak@.nl";

            for (int i = 0; i < wrongEmails.Length; i++)
            {
                var check = Regex.IsMatch(Convert.ToString(wrongEmails[i]).Trim(),
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

                Assert.AreEqual(false, check);
            }
        }
    }
}
