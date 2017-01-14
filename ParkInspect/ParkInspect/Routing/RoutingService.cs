using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMAPWPF.CustomMarkers;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ParkInspect.Routing
{
  public  class RoutingService
    {
        public List<String> GetDirections(string home_adress,string street,string region_zip,string region_name, out string errorMsg)
        {
            List<String> directionItems = new List<string>();
            if (String.IsNullOrWhiteSpace(home_adress))
            {
                errorMsg = "invalidAdress";
                return null;
            }
            else
            {
                //route on gmaps
                try
                {
                    var drivingDirectionRequest = new DirectionsRequest
                    {
                        Origin = home_adress,
                        Destination = street + " " + region_zip + " " + region_name
                    };
                    drivingDirectionRequest.Language = "nl";
                    DirectionsResponse drivingDirections = GoogleMaps.Directions.Query(drivingDirectionRequest);
                    Route nRoute = drivingDirections.Routes.First();
                    Leg leg = nRoute.Legs.First();
                    int counter = 1;
                    //direction items
                    directionItems.Clear();
                    foreach (Step step in leg.Steps)
                    {
                        directionItems.Add(counter + ". " + StripHTML(step.HtmlInstructions));
                        counter++;
                    }
                }
                catch (Exception e)
                {
                    directionItems.Clear();
                    errorMsg = "directionsFailed";
                    return null;
                }
            }
            errorMsg = "succes";
            return directionItems;
        }
        private string StripHTML(string html)
        {
            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
        public PointLatLng getPointFromKeyWord(String keyword)
        {
            GeoCoderStatusCode status;
            PointLatLng? point = GMapProviders.OpenStreetMap.GetPoint(keyword, out status);
            if (status == GeoCoderStatusCode.G_GEO_SUCCESS && point != null)
            {
                return new PointLatLng(point.Value.Lat, point.Value.Lng);
            }
            return new PointLatLng(0, 0);
        }

        public RouteObject GetRoute(PointLatLng start, PointLatLng end)
        {
            RoutingProvider rp;
            rp = GMapProviders.OpenStreetMap;
            MapRoute route = rp.GetRoute(start, end, false, false, 8);
            if (route != null)
            {
                GMapMarker m1 = new GMapMarker(start);
                m1.Shape = new CustomMarkerDemo(null, m1, "Start: " + route.Name);
                m1.Shape.IsEnabled = false;

                GMapMarker m2 = new GMapMarker(end);
                m2.Shape = new CustomMarkerDemo(null, m2, "End: " + start.ToString());
                m2.Shape.IsEnabled = false;

                GMapRoute mRoute = new GMapRoute(route.Points);
                {
                    mRoute.ZIndex = -1;
                }
                if (route.Distance > 0)
                {
                    RouteObject routeObject = new RouteObject(mRoute, m1, m2, route.Distance);
                    return routeObject;
                }
            }
            else
            {
                MessageBox.Show("Het navigeren vereist een internet verbinding!");
            }
            return null;   
        }
    }

}
