using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParkInspect.Routing
{
    class MyMarker
    {

        public GMapMarker gMapMarker;
        public object o;
        public UIElement shape;
        public MyMarker(GMapMarker gMapMarker, object o)
        {
            this.gMapMarker = gMapMarker;
            this.o = o;
            this.shape = gMapMarker.Shape;
        }
    }
}
