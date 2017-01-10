using GalaSoft.MvvmLight;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMAPWPF.CustomMarkers;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using ParkInspect.Repository;
using ParkInspect.Routing;
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
        String street;
        String number;
        String region;
        RoutingService routingService;
        public InspectionService service;
        private String runpath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        private PrepareViewModel vm;
        public PrepareControl(Inspection inspection)
        {
            InitializeComponent();
            vm = (PrepareViewModel)this.DataContext;
            service = vm.service;
            vm.selectedInspection = inspection;
            init();
        }
        public PrepareControl()
        {
            InitializeComponent();
            vm = (PrepareViewModel)this.DataContext;
            routingService = new RoutingService();
            init();

        }
        private void init()
        {
            routingService = new RoutingService();
            OpenStreetMapProvider.UserAgent = ".NET Framework Test Client";
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
                street = l_street_text.Content.ToString();
                zip = l_zip_text.Content.ToString();
                number = l_number_text.Content.ToString();
                region = l_region_text.Content.ToString();
                zip = zip.Replace(" ", "");
                zip = zip.Trim();
                PointLatLng start = routingService.getPointFromKeyWord(txt_home_adres.Text);
                PointLatLng end = routingService.getPointFromKeyWord(street + " " + zip + " " + region);
                RouteObject route = routingService.GetRoute(start, end);
                if(route != null)
                {
                    gmap.Markers.Add(route.m1);
                    gmap.Markers.Add(route.m2);
                    gmap.Markers.Add(route.route);
                    gmap.ZoomAndCenterMarkers(null);
                }


            }
        }
    }
}
