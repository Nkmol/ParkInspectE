using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;

namespace ParkInspect
{

    public class PdfBuilder
    {

        private Document _document { get; set; }
        private PdfWriter _writer { get; set; }

        public PdfBuilder(Stream stream)
        {
            _document = new Document();
            _writer = PdfWriter.GetInstance(_document, stream);
            _document.Open();
        }

        /*
         * Sets the size of the new document.
         * float width - the width of the document
         * float height - the height of the document
         */
        public void SetSize(float width, float height)
        {
            _document.SetPageSize(new Rectangle(width, height));
        }

        /*
         * Adds a table to the current document
         * string[] columns = All the columns to be shown in the table from the current dataset
         * string[] headers (optional) = All the cosmetic headers for the table. Length has to match columns length
         * PdfAlignment alignment (optional) - Tells the document where to align the table
         */
        public void AddTable<T>(IEnumerable<T> data, string[] columns, string[] headers = null, PdfAlignment alignment = PdfAlignment.LEFT)
        {

            if (headers != null && columns.Length != headers.Length)
                return;

            var t = typeof(T);
            var table = new PdfPTable(columns.Length);
            table.HorizontalAlignment = (int) alignment;

            var realColumns = (headers != null && headers.Length > 0 ? headers : columns);
            foreach(var column in realColumns)
            {
                table.AddCell(column);
            }

            foreach (var item in data)
            {

                foreach (var column in columns)
                {
                    var p = t.GetProperty(column);
                    table.AddCell((string)p.GetValue(item));
                }

            }

            _document.Add(table);

        }

        /*
         * Adds an image to the document.
         * string location - the location of the image (can be local or an URL)
         * int width (optional) - the scaled width of the image
         * int height (optional) - the scaled height of the image
         * PdfAlignment alignment (optional) - the alignment of the image on the document
         */
        public void AddImage(string location, int width = 0, int height = 0, PdfAlignment alignment = PdfAlignment.LEFT)
        {

           var img = iTextSharp.text.Image.GetInstance(location);
            img.Alignment = (int) alignment;

            if(width > 0 || height > 0)
                img.ScaleAbsolute(width, height);

            _document.Add(img);

        }

        /*
         * Writes the document. Basically closes all used variables.
         */
        public void Build()
        {
            _writer.Flush();
            _document.Close();
        }

    }

    /*
     * An enumeration for the Element constants.
     */
    public enum PdfAlignment
    {

        RIGHT = Element.ALIGN_RIGHT,
        LEFT = Element.ALIGN_LEFT,
        CENTER = Element.ALIGN_CENTER,
        TOP = Element.ALIGN_TOP,
        BOTTOM = Element.ALIGN_BOTTOM,
        MIDDLE = Element.ALIGN_MIDDLE,
        BASELINE = Element.ALIGN_BASELINE,
        JUSTIFIED = Element.ALIGN_JUSTIFIED,
        JUSTIFIED_ALL = Element.ALIGN_JUSTIFIED_ALL

    }
}
