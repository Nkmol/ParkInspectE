using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Routing
{
    public class RouteObject
    {
        public GMapRoute route;
        public GMapMarker m1;
        public GMapMarker m2;
        public double distance;
        public RouteObject(GMapRoute route, GMapMarker m1, GMapMarker m2, double distance)
        {
            this.route = route;
            this.m1 = m1;
            this.m2 = m2;
            this.distance = distance;
        }
    }
}
