using GalaSoft.MvvmLight;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMAPWPF.CustomMarkers;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using ParkInspect.Repository;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParkInspect.View.UserControls
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class PrepareControl : UserControl
    {
        String zip;
        String region;
        int inspection_id;
        String home_adress;
        Inspection inspection;
        public InspectionService service;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private PrepareViewModel vm;
        public PrepareControl()
        {
            InitializeComponent();
            vm = (PrepareViewModel)this.DataContext;

            service = vm.service;
            inspection = new Inspection();
            OpenStreetMapProvider.UserAgent = ".NET Framework Test Client";
            GMaps.Instance.ImportFromGMDB("db.gmdb");
            gmap.MapProvider = OpenStreetMapProvider.Instance;
            gmap.Manager.Mode = AccessMode.ServerAndCache;
            gmap.Manager.UseGeocoderCache = true;
            gmap.SetPositionByKeywords("Amsterdam");
            gmap.MinZoom = 1;
            gmap.MaxZoom = 18;
            gmap.Zoom = 8;
            gmap.ShowCenter = false;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            gmap.Markers.Clear();
            if (!String.IsNullOrWhiteSpace(zip) || !String.IsNullOrWhiteSpace(region) ||
                !String.IsNullOrWhiteSpace(txt_home_adres.Text))
            {
                zip = l_zip_text.Content.ToString();
                region = l_region_text.Content.ToString();
                zip = zip.Trim();
                PointLatLng start = getPointFromKeyWord(txt_home_adres.Text);
                PointLatLng end = getPointFromKeyWord(zip + " " + region);

                RoutingProvider rp = gmap.MapProvider as RoutingProvider;
                if (rp == null)
                {
                    rp = GMapProviders.OpenStreetMap;
                    ; // use OpenStreetMap if provider does not implement routing
                }

                MapRoute route = rp.GetRoute(start, end, false, false, (int)gmap.Zoom);
                if (route != null)
                {
                    GMapMarker m1 = new GMapMarker(start);
                    m1.Shape = new CustomMarkerDemo(this, m1, "Start: " + route.Name);
                    m1.Shape.IsEnabled = false;

                    GMapMarker m2 = new GMapMarker(end);
                    m2.Shape = new CustomMarkerDemo(this, m2, "End: " + start.ToString());
                    m2.Shape.IsEnabled = false;

                    GMapRoute mRoute = new GMapRoute(route.Points);
                    {
                        mRoute.ZIndex = -1;
                    }

                    gmap.Markers.Add(m1);
                    gmap.Markers.Add(m2);
                    gmap.Markers.Add(mRoute);

                    gmap.ZoomAndCenterMarkers(null);
                    l_distance.Content = "Afstand: " + Math.Round(route.Distance, 1) + "KM";
                }
                else
                {
                    MessageBox.Show("Het navigeren vereist een internet verbinding!");
                }

            }
        }


        private PointLatLng getPointFromKeyWord(String keyword)
        {
            GeoCoderStatusCode status;
            PointLatLng? point = GMapProviders.OpenStreetMap.GetPoint(keyword, out status);
            if (status == GeoCoderStatusCode.G_GEO_SUCCESS && point != null)
            {
                return new PointLatLng(point.Value.Lat, point.Value.Lng);
            }
            return new PointLatLng(0, 0);
        }
    }
}
