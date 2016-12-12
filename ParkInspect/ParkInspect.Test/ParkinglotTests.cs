using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect.Repository;
using ParkInspect.ViewModel;

namespace ParkInspect.Test
{
    [TestClass]
    public class ParkinglotTests
    {
        private EntityFrameworkRepository<ParkInspectEntities> _repo;
        private ParkinglotViewModel vm;

        [TestInitialize]
        public void init()
        {
            _repo = new EntityFrameworkRepository<ParkInspectEntities>(new ParkInspectEntities());
            vm = new ParkinglotViewModel(_repo);
        }
        
        [TestMethod]
        public void Create()
        {          
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 45
            };

            var pl = _repo.GetOne<Parkinglot>(p => p.name == plot.name);

            if (pl != null)
            {
                _repo.Delete(pl);
            }

            vm.Parkinglot = plot;
            vm.SaveCommand.Execute(vm.Parkinglot);
            Assert.AreEqual("The parkinglot was added!", vm.Message);
        }

        [TestMethod]
        public void CreateWrong()
        {
            var plot = new Parkinglot
            {
                name = "TestCreateWrong",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 45
            };

            vm.Parkinglot = plot;
            vm.SaveCommand.Execute(vm.Parkinglot);
            Assert.AreEqual("Something went wrong.", vm.Message);
        }

        [TestMethod]
        public void Update()
        {
            var parkinglot = new Parkinglot
            {
                name = "TestUnitUpdate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Testunit parker",
                number = 45
            };
            
            var plot = _repo.GetOne<Parkinglot>(p => p.name.ToLower() == parkinglot.name.ToLower());

            if (plot == null)
            {
                _repo.Create(parkinglot);
                _repo.Save();
            }

            plot.name = "Nieuwe naam";
            vm.Parkinglot = plot;         
            vm.SaveCommand.Execute(vm.Parkinglot);
            Assert.AreEqual("The parkinglot was updated!", vm.Message);
        }

        [TestMethod]
        public void UpdateWrong()
        {
            var parkinglot = new Parkinglot
            {
                name = "TestUnitUpdateWrong",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Testunit parker",
                number = 45
            };

            var pl = _repo.GetOne<Parkinglot>(p => p.name == parkinglot.name);

            if (pl != null)
            {
                _repo.Delete(pl);
            }

            _repo.Create(parkinglot);
            _repo.Save();

            var plot = _repo.GetOne<Parkinglot>(p => p.name.ToLower() == parkinglot.name.ToLower());
            plot.id = -2;
            vm.Parkinglot = plot;
            vm.SaveCommand.Execute(vm.Parkinglot);
            Assert.AreEqual("Something went wrong.", vm.Message);
        }

        [TestMethod]
        public void Read()
        {
            var parkinglot = new Parkinglot
            {
                name = "TestUnitRead",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "TestunitRead parker",
                number = 45
            };

            var pl = _repo.GetOne<Parkinglot>(p => p.name == parkinglot.name);

            if (pl != null)
            {
                _repo.Delete(pl);
            }

            _repo.Create(parkinglot);
            _repo.Save();

            var plot = _repo.GetOne<Parkinglot>(p => p.name == parkinglot.name);

            Assert.AreEqual(parkinglot.name, plot.name);
        }

        [TestMethod]
        public void wrongZipcode()
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
        public void rightZipcode()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 45
            };

            var isZipCode = Regex.IsMatch(Convert.ToString(plot.zipcode), @"^[1-9][0-9]{3}\s?[A-Z]{2}$");

            Assert.AreEqual(true, isZipCode);
        }

        [TestMethod]
        public void wrongPosNumber()
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
        public void isPosNumber()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 10
            };

            var isPosNumber = plot.number > 0;

            Assert.AreEqual(true, isPosNumber);
        }

        [TestMethod]
        public void NameIsNotEmpty()
        {
            var plot = new Parkinglot
            {
                id = -1,
                name = "TestCreate",
                zipcode = "2020GH",
                region_name = "Gelderland",
                clarification = "Test parker",
                number = 10
            };

            var val = Convert.ToString(plot.name).Trim();

            var nameIsNotEmpty = !string.IsNullOrEmpty(val);

                Assert.AreEqual(true, nameIsNotEmpty);
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
        public void NameFilter()
        {
            string filter = "henk";

            vm.NameFilter = filter;

            var list1 = new ObservableCollection<Parkinglot>();

            foreach (var pl in vm.Parkinglots)
            {
                list1.Add(pl);
            }

            vm.UpdateParkinglots();

            
            var list2 = new ObservableCollection<Parkinglot>();

            foreach (var pl in vm.Parkinglots)
            {
                list1.Add(pl);
            }

            //Assert.AreEqual(list, vm.Parkinglots);
        }
    }
}