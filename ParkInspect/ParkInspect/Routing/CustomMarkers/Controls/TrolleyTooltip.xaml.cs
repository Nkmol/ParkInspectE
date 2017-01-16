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

namespace ParkInspect.View.Controls
{
    /// <summary>
    /// Interaction logic for TrolleyTooltip.xaml
    /// </summary>
    public partial class TrolleyTooltip : UserControl
    {
        Dictionary<string,string> statusConverter;
        public TrolleyTooltip()
        {
            InitializeComponent();
            statusConverter = new Dictionary<string, string>();
            statusConverter.Add("Finished", "Voltooid");
            statusConverter.Add("Halted", "Gestopt");
            statusConverter.Add("In progress", "In voortgang");
            statusConverter.Add("Unbegun", "Onbegonnen");

        }
        public void SetValues(object o, int inspections = 0)
        {
            if (o is Inspection)
            {
                Inspection i = (Inspection)o;
                Place.Content = "Plaats: " + i.Parkinglot.Region.name;
                Name.Content = "Naam: " + i.Parkinglot.name;
                Status.Content = "Status: " + statusConverter[i.state];
            }
            if (o is Absence)
            {
                Absence a = (Absence)o;
                Place.Content = "Naam: " + a.Employee.firstname + " " + a.Employee.lastname;
                Name.Content = a.start + " - " + a.end;
                Status.Content = "Regio : " + a.Employee.Region.name;
            }
            if (o is Parkinglot)
            {
                Parkinglot p = (Parkinglot)o;
                Place.Content = "Naam: " + p.name;
                Name.Content = "Aantal inspecties: " + p.Inspections.Count;
                Status.Content = "Regio: " + p.Region.name;
            }
            if (o is Region)
            {
                Region r = (Region)o;
                Place.Content = "Regio: " + r.name;
                Name.Content = "Aantal inspecties: " + inspections;
                Status.Content = "";
            }

        }
    }
}
