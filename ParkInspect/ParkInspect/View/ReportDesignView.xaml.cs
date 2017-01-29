using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Syncfusion.RDL.DOM;
using Syncfusion.Windows.Reports.Designer;

namespace ParkInspect
{
    /// <summary>
    /// Interaction logic for ReportDesignView.xaml
    /// </summary>
    public partial class ReportDesignView : Window
    {
        public ReportDesignView()
        {
            InitializeComponent();

            Designer.ReportLoaded += (sender, args) =>
            {

                var stream = Designer.GetReportStream();
                

                //Serialize the contents from the file stream to ReportDefinition to edit the ReportDefinition
                XElement rdl = XElement.Load(XmlReader.Create(stream));
                string Namespace = (from attribute in rdl.Attributes() where attribute.Name.LocalName == "xmlns" select attribute.Value).FirstOrDefault();
                XmlSerializer xs = new XmlSerializer(typeof(Syncfusion.RDL.DOM.ReportDefinition), Namespace);
                Syncfusion.RDL.DOM.ReportDefinition report;
                using (StringReader reader = new StringReader(rdl.ToString()))
                {
                    report = (Syncfusion.RDL.DOM.ReportDefinition)xs.Deserialize(reader);
                }

                DataSource datasrc = new DataSource();
                datasrc.Name = "ParkInspect";
                datasrc.ConnectionProperties = new ConnectionProperties();
                datasrc.ConnectionProperties.DataProvider = "SQL";
                datasrc.ConnectionProperties.ConnectString = "Data Source=bhooff.database.windows.net;Initial Catalog=ParkInspect;user id=bhooff;password=Wachtwoord123";
                datasrc.ConnectionProperties.IntegratedSecurity = true;//for windows authentication.

                if (report.DataSources == null)
                {
                    report.DataSources = new DataSources();
                }

                report.DataSources.Add(datasrc);

            };
        }
    }
}
