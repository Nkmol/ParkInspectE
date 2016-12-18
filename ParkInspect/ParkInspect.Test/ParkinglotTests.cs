using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect.Services;
using UnitTestProject;

namespace ParkInspect.Test
{
    [TestClass]
    public class ParkinglotTests
    {
        private MockRepositoryWrapper<Parkinglot, ParkInspectEntities> _mockRepo;
        private Parkinglot _newParkinglot;
        private ParkinglotService _service;

        [TestInitialize]
        public void Init()
        {
            _mockRepo = new MockRepositoryWrapper<Parkinglot, ParkInspectEntities>();
            _service = new ParkinglotService(_mockRepo.Repo);

            // Arrange
            _newParkinglot = new Parkinglot()
            {
                id = -1,
                name = "Test parkeerplaats",
                clarification = "Dit is een test parkeerplaats",
                number = 45,
                region_name = "Gelderland",
                zipcode = "5555GG"
            };

            _service.Add(_newParkinglot);
        }
        
        [TestMethod]
        public void Create()
        {
            //Arrange
            var gelderland = new Region {name = "Gelderland"};
            var pl = new Parkinglot()
            {
                id = -1,
                name = "TestCreate parkeerplaats",
                clarification = "Dit is een test parkeerplaats",
                number = 45,
                Region = gelderland,
                region_name = gelderland.name,
                zipcode = "5555GG"
            };

            //Act
            _service.Add(pl);

            //Assert
            var parkinglot = _service.Get(pl);
            Assert.AreEqual("TestCreate parkeerplaats", parkinglot.name);
        }

        [TestMethod]
        public void Update()
        {
            //Arrange
            _newParkinglot.name = "Update parkeerplaats";

            //Act
            _service.Update(_newParkinglot);

            //Assert
            var pl = _service.Get(_newParkinglot);
            Assert.AreEqual("Update parkeerplaats", pl.name);
        }

        [TestMethod]
        public void Read()
        {
            var parkinglot = _service.GetParkinglot(_newParkinglot.name);

            Assert.IsNotNull(parkinglot);
        }

        [TestMethod]
        public void WrongZipcode()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GHs",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 45
            };

            var isZipCode = Regex.IsMatch(Convert.ToString(plot.zipcode), @"^[1-9][0-9]{3}\s?[A-Z]{2}$");

            Assert.AreEqual(false, isZipCode);
        }

        [TestMethod]
        public void WrongNumber()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = -10
            };

            var isPosNumber = plot.number > 0;

            Assert.AreEqual(false, isPosNumber);
        }

        [TestMethod]
        public void NameIsEmpty()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 10
            };

            var val = Convert.ToString(plot.name).Trim();

            var nameIsNotEmpty = !string.IsNullOrEmpty(val);

            Assert.AreEqual(false, nameIsNotEmpty);
        }

        [TestMethod]
        public void RegionIsEmpty()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestRegion",
                zipcode = "2020GH",
                region_name = "",
                clarification = "Test parker",
                number = 10
            };

            var val = Convert.ToString(plot.region_name).Trim();

            var nameIsNotEmpty = !string.IsNullOrEmpty(val);

            Assert.AreEqual(false, nameIsNotEmpty);
        }
    }
}