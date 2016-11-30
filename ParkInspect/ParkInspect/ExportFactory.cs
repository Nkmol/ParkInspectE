using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using DataGrid = System.Windows.Controls.DataGrid;

namespace ParkInspect
{
    class ExportFactory
    {

        public static void ExportCsv<T>(IEnumerable<T> data, string[] columns)
        {

            Type t = typeof(T);
            string csv = "";

            csv += WriteHeaders(columns);

            foreach (var item in data)
            {

                foreach(var column in columns)
                {
                    PropertyInfo p = t.GetProperty(column);
                    csv += (string)p.GetValue(item) + ",";
                }
                csv += "\n";
            }


            csv.Trim();

            StreamWriter swObj = new StreamWriter("exportToExcel.csv");
            swObj.WriteLine(csv);
            swObj.Close();
            Process.Start("exportToExcel.csv");

        }

        private static string WriteHeaders(string[] columns)
        {

            string headers = "";

            foreach (var column in columns)
            {
                //Csv is seperated by ,
                headers += column + ",";
            }

            //Csv new line is declared with \n
            headers += "\n";

            return headers;

        }


    }
}
