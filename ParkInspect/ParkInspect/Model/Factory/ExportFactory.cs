using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkInspect.Model.Factory
{
    class ExportFactory
    {
        private static Stream _stream;

        /*
         * Save dataset to CSV.
         * @param data - Dataset that should be exported
         * @param columns - The columns that should be executed, will have to match the dataset!
         * @param headers (optional) - The shown headers of the table. 
         */
        public static void ExportCsv<T>(IEnumerable<T> data, string[] columns, string[] headers = null)
        {
            if (columns.Length == 0 || data == null)
                return;

            if (PromptSave("CSV (*.csv)"))
            {
                Type t = typeof(T);
                var csv = new StringBuilder();

                csv.Append(WriteHeaders(headers != null ? headers : columns));

                foreach (var item in data)
                {
                    foreach (var column in columns)
                    {
                        PropertyInfo p = t.GetProperty(column);
                        csv.Append(p.GetValue(item) + ",");
                    }
                    csv.Append("\n");
                }

                StreamWriter sw = new StreamWriter(_stream);
                sw.WriteLine(csv);
                sw.Close();
            }
        }

        /*
         * Save dataset to PDF.
         * @param data - Dataset that should be exported
         * @param columns (optional) - The columns that should be executed, will have to match the dataset!
         * @param headers (optional) - The shown headers of the table. 
         */
        public static void ExportPdf<T>(IEnumerable<T> data, Type type = null, string[] columns = null, string[] headers = null)
        {
            if (!PromptSave("PDF Files | *.pdf"))
                return;

            string applicationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string rootPath = Directory.GetParent(applicationDirectory).Parent.FullName;

            var builder = new PdfBuilder(_stream);
            builder.AddImage(rootPath + "/ParkInspect.png", 447, 204, PdfAlignment.CENTER);
            builder.AddTable(data, type, columns, headers, PdfAlignment.CENTER);
            builder.Build();
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

        private static bool PromptSave(string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = filter;
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                return false;

            }

            _stream = saveFileDialog.OpenFile();
            return true;
        }
    }
}