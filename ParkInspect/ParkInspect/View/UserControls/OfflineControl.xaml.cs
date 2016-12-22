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
using ParkInspect.Model;

namespace ParkInspect.View.UserControls
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class OfflineControl : UserControl
    {
        String zip;
        String region;
        int inspection_id;
        String home_adress;
        Inspection inspection;
        public InspectionService service;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private OfflineViewModel vm;
        public OfflineControl()
        {
            InitializeComponent();
            vm = (OfflineViewModel)this.DataContext;

            service = vm.service;
            inspection = new Inspection();
            OpenStreetMapProvider.UserAgent = ".NET Framework Test Client";
            gmap_offline.MapProvider = OpenStreetMapProvider.Instance;
            gmap_offline.Manager.Mode = AccessMode.ServerAndCache;
            gmap_offline.Manager.UseGeocoderCache = true;
            gmap_offline.SetPositionByKeywords("Amsterdam");
            gmap_offline.MinZoom = 1;
            gmap_offline.MaxZoom = 18;
            gmap_offline.Zoom = 8;
            gmap_offline.ShowCenter = false;
        }
        private void ReadInspectionInfoFromFile()
        {
            String line;
            String name = (listBox1.SelectedItem as Direction).Name;
            String path = runpath + "/directions/" + name + ".txt";
           System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("ID:"))
                {
                    inspection_id = Convert.ToInt32(line.Replace("ID:", ""));
                }
                if (line.Contains("HOME:"))
                {
                    home_adress = line.Replace("HOME:", "");
                }
            }

        }

        private void GetInspectionInfo()
        {
            inspection = service.GetInspectionByID(inspection_id);
            vm.selectedInspection = inspection;
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
        private void loadRoute()
        {
            ReadInspectionInfoFromFile();
            GetInspectionInfo();
            zip = inspection.Parkinglot.zipcode;
            region = inspection.Parkinglot.Region.name;
            gmap_offline.Markers.Clear();
            if (!String.IsNullOrWhiteSpace(zip) || !String.IsNullOrWhiteSpace(region) ||
                !String.IsNullOrWhiteSpace(home_adress))
            {

                zip = zip.Trim();
                PointLatLng start = getPointFromKeyWord(home_adress);
                PointLatLng end = getPointFromKeyWord(zip + " " + region);

                RoutingProvider rp = gmap_offline.MapProvider as RoutingProvider;
                if (rp == null)
                {
                    rp = GMapProviders.OpenStreetMap;
                    ; // use OpenStreetMap if provider does not implement routing
                }

                MapRoute route = rp.GetRoute(start, end, false, false, (int)gmap_offline.Zoom);
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

                    gmap_offline.Markers.Add(m1);
                    gmap_offline.Markers.Add(m2);
                    gmap_offline.Markers.Add(mRoute);

                    gmap_offline.ZoomAndCenterMarkers(null);
                    // l_distance.Content = "Afstand: " + Math.Round(route.Distance, 1) + "KM";
                }
                else
                {
                    MessageBox.Show("Het navigeren vereist een internet verbinding!");
                }

            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadRoute();   
        }
    
    }
}
