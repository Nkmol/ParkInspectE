using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using ParkInspect.Services;
using ParkInspect.ViewModel;
using ParkInspect.Routing;
using ParkInspect.Model;
using ParkInspect.Routing.CustomMarkers;

namespace ParkInspect.View.UserControls
{
    /// <summary>
    /// Interaction logic for ReportControl.xaml
    /// </summary>
    public partial class ReportControl : UserControl
    {
        private ReportViewModel vm;
        Dictionary<UIElement, MyMarker> markerMap = new Dictionary<UIElement, MyMarker>();
        InspectionService _inspetionservice;
        AbsenceService _absenservice;
        ParkinglotService _parkinglotservice;
        private RoutingService routingService;
        public ReportControl()
        {

            InitializeComponent();
            routingService = new RoutingService();
            vm = (ReportViewModel)this.DataContext;
            _inspetionservice = vm.inspection_service;
            _absenservice = vm.absence_service;
            _parkinglotservice = vm.parkinglot_service;
            OpenStreetMapProvider.UserAgent = ".NET Framework Test Client";
            GMaps.Instance.ImportFromGMDB("db.gmdb");
            initGmap(gmap);
            //initInspections();
            initHeatMap();
            comboBox.SelectedIndex = 0;
        }
        private void initGmap(GMapControl gmap)
        {
            gmap.MapProvider = OpenStreetMapProvider.Instance;
            gmap.Manager.Mode = AccessMode.ServerAndCache;
            gmap.Manager.UseGeocoderCache = true;
            gmap.SetPositionByKeywords("Amsterdam");
            gmap.MinZoom = 1;
            gmap.MaxZoom = 18;
            gmap.Zoom = 8;
            gmap.ShowCenter = false;

        }
        private void initParkingLots()
        {
            gmap.Markers.Clear();
            foreach (Parkinglot p in loadParkinglots())
            {
                addParkinglotMarker(p, Brushes.Red);
            }
        }
        private void initAbsences()
        {
            gmap.Markers.Clear();
            foreach (Absence a in loadAbsences())
            {
                addAbsenceMarker(a, Brushes.Red);
            }
        }
        private void initInspections()
        {
            gmap.Markers.Clear();
            foreach (ParkInspect.Inspection i in loadInspections())
            {
                switch (i.state)
                {
                    case "Finished":
                        addInspectionMarker(i, Brushes.Green);
                        break;
                    case "In progress":
                        addInspectionMarker(i, Brushes.Orange);
                        break;
                    case "Halted":
                        addInspectionMarker(i, Brushes.Red);
                        break;
                    case "Unbegun":
                        addInspectionMarker(i, Brushes.FloralWhite);
                        break;

                }
            }
        }
        private void initHeatMap()
        {
            gmap.Markers.Clear();
            Dictionary<Region, int> inspection_freq = new Dictionary<Region, int>();
            foreach (ParkInspect.Inspection i in loadInspections())
            {
                if (inspection_freq.ContainsKey(i.Parkinglot.Region))
                {
                    inspection_freq[i.Parkinglot.Region]++;
                }
                else
                {
                    inspection_freq.Add(i.Parkinglot.Region, 1);
                }

                foreach (Region r in inspection_freq.Keys)
                {
                    addHeatMapMarker(r, inspection_freq[r]);
                }
            }
        }
        private void addInspectionMarker(ParkInspect.Inspection inspection, Brush color)
        {
            String zip = inspection.Parkinglot.zipcode.Trim();
            String region = inspection.Parkinglot.Region.name;
            GMapMarker marker = new GMapMarker(routingService.getPointFromKeyWord(inspection.Parkinglot.streetname + " " + zip + " " + region));
            if (marker.Position.Lat != 0 || marker.Position.Lng != 0)
            {
                var shape = new CircleVisual(marker, color);
                shape.Tooltip.SetValues(inspection);
                marker.Shape = shape;
                MyMarker inspectionMarker = new MyMarker(marker, inspection);

                markerMap.Add(marker.Shape, inspectionMarker);
                gmap.Markers.Add(inspectionMarker.gMapMarker);
            }
        }
        private void addHeatMapMarker(Region r, int size)
        {
            GMapMarker marker = new GMapMarker(routingService.getPointFromKeyWord(r.name));
            if (marker.Position.Lat != 0 || marker.Position.Lng != 0)
            {
                var shape = new CircleVisual(marker, Brushes.Red, 11 + 2 * size);
                shape.Tooltip.SetValues(r, size);
                marker.Shape = shape;
                MyMarker Marker = new MyMarker(marker, r);

                markerMap.Add(marker.Shape, Marker);
                gmap.Markers.Add(Marker.gMapMarker);
            }
        }
        private void addAbsenceMarker(Absence absence, Brush color)
        {
            String region = absence.Employee.Region.name;
            GMapMarker marker = new GMapMarker(routingService.getPointFromKeyWord(region));
            if (marker.Position.Lat != 0 || marker.Position.Lng != 0)
            {
                var shape = new CircleVisual(marker, color);
                shape.Tooltip.SetValues(absence);
                marker.Shape = shape;
                MyMarker Marker = new MyMarker(marker, absence);

                markerMap.Add(marker.Shape, Marker);
                gmap.Markers.Add(Marker.gMapMarker);
            }
        }
        private void addParkinglotMarker(Parkinglot p, Brush color)
        {
            String zip = p.zipcode.Trim();
            String region = p.Region.name;
            GMapMarker marker = new GMapMarker(routingService.getPointFromKeyWord(p.streetname + " " + zip + " " + region));
            if (marker.Position.Lat != 0 || marker.Position.Lng != 0)
            {
                var shape = new CircleVisual(marker, color);
                shape.Tooltip.SetValues(p);
                marker.Shape = shape;
                MyMarker Marker = new MyMarker(marker, p);

                markerMap.Add(marker.Shape, Marker);
                gmap.Markers.Add(Marker.gMapMarker);
            }
        }
        private List<ParkInspect.Inspection> loadInspections()
        {
            return new List<ParkInspect.Inspection>(_inspetionservice.GetAllInspections());
        }
        private List<Parkinglot> loadParkinglots()
        {
            return new List<Parkinglot>(_parkinglotservice.GetAllParkinglots());
        }
        private List<Absence> loadAbsences()
        {
            return new List<Absence>(_absenservice.GetAllAbsences());
        }
        public void Select(object sender, MouseButtonEventArgs e)
        {

            ((ReportViewModel)DataContext).OpenReportView(Reports.SelectedIndex);
        }
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                initInspections();
            }
            if (comboBox.SelectedIndex == 1)
            {
                initAbsences();
            }
            if (comboBox.SelectedIndex == 2)
            {
                initParkingLots();
            }
            if (comboBox.SelectedIndex == 3)
            {
                initHeatMap();
            }
        }
    }
}
