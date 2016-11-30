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
using iTextSharp.text;
using iTextSharp.text.pdf;
using DataGrid = System.Windows.Controls.DataGrid;

namespace ParkInspect
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
         * @param columns - The columns that should be executed, will have to match the dataset!
         * @param headers (optional) - The shown headers of the table. 
         */
        public static void ExportPdf<T>(IEnumerable<T> data, string[] columns, string[] headers = null)
        {

            if (PromptSave("PDF (*.pdf)"))
            {
                Type t = typeof(T);

                string applicationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string rootPath = Directory.GetParent(applicationDirectory).Parent.FullName;


                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, _stream);
                doc.Open();

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(rootPath + "/ParkInspect.png");
                img.ScaleAbsolute(449, 204);
                doc.Add(img);

                PdfPTable table = new PdfPTable(columns.Length);

                foreach (var column in columns)
                {
                    table.AddCell(column);
                }

                foreach (var item in data)
                {

                    foreach (var column in columns)
                    {
                        PropertyInfo p = t.GetProperty(column);
                        table.AddCell((string)p.GetValue(item));
                    }

                }

                doc.Add(table);
                writer.Flush();
                doc.Close();

            }
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
