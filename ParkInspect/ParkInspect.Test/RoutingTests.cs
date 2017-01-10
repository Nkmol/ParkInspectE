using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect;
using ParkInspect.Services;
using ParkInspect.Routing;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class RoutingTests
    {
        private RoutingService routingService ;

        [TestInitialize]
        public void Init()
        {
            routingService = new RoutingService();
        }

        // Small/single asserts are good!
        // http://softwareengineering.stackexchange.com/questions/7823/is-it-ok-to-have-multiple-asserts-in-a-single-unit-test

        [TestMethod]
        [TestCategory("Routing service")]
        public void GetGoodDirections()
        {
           
            string home_adress = "Waaloord 102";
            string street = "Arena";
            string region_zip = "5211 XT";
            string region_name = "Noord-Brabant";
            string errorMsg;
            List<string> list = routingService.GetDirections(home_adress, street, region_zip, region_name, out errorMsg);
            Assert.IsTrue(list != null, "The direction items list was empty! (no directions where found)");
        }
        [TestMethod]
        [TestCategory("Routing service")]
        public void GetFailedDirections()
        {
            string home_adress = "asfsadfasdf";
            string street = "Arena";
            string region_zip = "5211 XT";
            string region_name = "Noord-Brabant";
            string errorMsg;
            List<string> list = routingService.GetDirections(home_adress, street, region_zip, region_name, out errorMsg);
            Assert.AreEqual(errorMsg, "directionsFailed","The error message was not the expected one");
        }
    }
}