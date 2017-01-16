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
using ParkInspect.Routing;

namespace ParkInspect.View.UserControls
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class OfflineControl : UserControl
    {
        String zip;
        String region;
        String street;
        int inspection_id;
        String home_adress;
        ParkInspect.Inspection inspection;
        public InspectionService service;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private OfflineViewModel vm;
        private RoutingService routingService;
        private bool clear = false;

        public OfflineControl()
        {
            InitializeComponent();
            vm = (OfflineViewModel)this.DataContext;
            routingService = new RoutingService();

            service = vm.service;
            inspection = new ParkInspect.Inspection();
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
            if (System.IO.File.Exists(path))
            {
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

        }
        private void loadRoute()
        {
            if (!clear)
            {
                ReadInspectionInfoFromFile();
                zip = vm.region_zip;
                region = vm.region_name;
                street = vm.street;
                gmap_offline.Markers.Clear();
                if (!String.IsNullOrWhiteSpace(zip) || !String.IsNullOrWhiteSpace(region) ||
                    !String.IsNullOrWhiteSpace(home_adress))
                {
                    zip = zip.Replace(" ", "");
                    zip = zip.Trim();
                    PointLatLng start = routingService.getPointFromKeyWord(home_adress);
                    PointLatLng end = routingService.getPointFromKeyWord(street + " " + zip + " " + region);
                    RouteObject route = routingService.GetRoute(start, end);
                    if (route != null)
                    {
                        gmap_offline.Markers.Add(route.m1);
                        gmap_offline.Markers.Add(route.m2);
                        gmap_offline.Markers.Add(route.route);
                        gmap_offline.ZoomAndCenterMarkers(null);
                    }
                }
            }
            clear = false;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadRoute();   
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            clear = true;
            gmap_offline.Markers.Clear();
        }
    }
}
