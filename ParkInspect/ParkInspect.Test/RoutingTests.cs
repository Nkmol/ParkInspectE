using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkInspect.Routing;
using System.Collections.Generic;
using GMap.NET;

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
        [TestMethod]
        [TestCategory("Routing service")]
        public void GetGoodRoute()
        {
            string home_adress = "Waaloord 102";
            string street = "Arena";
            string region_zip = "5211XT";
            string region_name = "Noord-Brabant";

            PointLatLng start = routingService.getPointFromKeyWord(home_adress);
            PointLatLng end = routingService.getPointFromKeyWord(street + " " + region_zip + " " + region_name);
            RouteObject route = routingService.GetRoute(start, end);
            Assert.IsTrue(route.m1.Position.Lat != 0, "The lat of marker m1 is 0!");
            Assert.IsTrue(route.m1.Position.Lng != 0, "The lng of marker m1 is 0!");
            Assert.IsTrue(route.m2.Position.Lat != 0, "The lat of marker m2 is 0!");
            Assert.IsTrue(route.m2.Position.Lng != 0, "The lng of marker m2 is 0!");


        }
        [TestMethod]
        [TestCategory("Routing service")]
        public void GetRouteInvalidStartAdress()
        {
            string home_adress = "asdasd";
            string street = "Arena";
            string region_zip = "5211XT";
            string region_name = "Noord-Brabant";

            PointLatLng start = routingService.getPointFromKeyWord(home_adress);
            PointLatLng end = routingService.getPointFromKeyWord(street + " " + region_zip + " " + region_name);
            RouteObject route = routingService.GetRoute(start, end);
            Assert.IsTrue(route == null);
        }
        [TestMethod]
        [TestCategory("Routing service")]
        public void GetRouteInvalidEndAdress()
        {
            string home_adress = "Waaloord 102";
            string street = "Arenaasdads";
            string region_zip = "5211XT";
            string region_name = "Noord-Brabant";

            PointLatLng start = routingService.getPointFromKeyWord(home_adress);
            PointLatLng end = routingService.getPointFromKeyWord(street + " " + region_zip + " " + region_name);
            RouteObject route = routingService.GetRoute(start, end);
            Assert.IsTrue(route == null);
        }

    }
}