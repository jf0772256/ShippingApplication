using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shipapp.Models;
using shipapp.Models.ModelData;
using shipapp.Connections.DataConnections;
using System.Windows.Forms;
using PdfSharp;
using MigraDoc;
using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Pdf.Printing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Actions;
using PdfSharp.Pdf.Content;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.Rendering.Printing;
using MigraDoc.RtfRendering;
using PdfSharp.Pdf;

namespace shipapp.Connections.HelperClasses
{
    class PrintDailyLog 
    {
        private List<Package> DeliverPackageList { get; set; }
        private User DeliveryPerson { get; set; }
        private PdfDocument PDF { get; set; }
        private XUnit PageHeight { get; set; }
        private XUnit PageWidth { get; set; }
        private PageOrientation orientation = PageOrientation.Landscape;
        private PageSize sizeofpage = PageSize.Letter;
        private int PagesCount { get; set; }
        //Fonts for sections type
        private PageFont PageHeading { get; set; }
        private PageFont ColumnHeading { get; set; }
        private PageFont PageContent { get; set; }


        public PrintDailyLog(List<Package> tobeDelivered)
        {
            DeliverPackageList = tobeDelivered;
            PDF = new PdfDocument();
            PageHeading = new PageFont();
            ColumnHeading = new PageFont();
            PageContent = new PageFont();
        }
        public PrintDailyLog(List<Package> tobeDelivered, User deliveredBy)
        {
            DeliverPackageList = tobeDelivered;
            DeliveryPerson = deliveredBy;
            PDF = new PdfDocument();
            PageHeading = new PageFont();
            ColumnHeading = new PageFont();
            PageContent = new PageFont();
        }
        public PrintDailyLog(List<Package> tobeDelivered, long deliveredBy)
        {
            DeliverPackageList = tobeDelivered;
            DeliveryPerson = DataConnectionClass.UserConn.Get1User(deliveredBy);
            PDF = new PdfDocument();
            PageHeading = new PageFont();
            ColumnHeading = new PageFont();
            PageContent = new PageFont();
        }

        public PdfPage CreatePDFPage()
        {
            PdfPage p = new PdfPage();
            p.Orientation = orientation;
            p.Size = sizeofpage;
            PageWidth = p.Height;
            PageHeight = p.Width;
            PageHeading.SetFontValue(PageFont.SetFont.MSSANS12B);
            ColumnHeading.SetFontValue(PageFont.SetFont.MSSANS10B);
            PageContent.SetFontValue(PageFont.SetFont.MSSANS10);
            XGraphics gfx = XGraphics.FromPdfPage(p);
            /**
             * TODO:: 
             * create border box
             * add header
             * add col headers
             * add content
             * return
             **/
            return p;
        }
        private PdfPage CreateBorderBox(PdfPage p)
        {
            return p;
        }
    }
    internal class PageFont
    {
        private SetFont Font { get; set; }
        internal System.Drawing.Font GetFont { get; private set; }
        internal PageFont()
        {

        }
        /// <summary>
        /// Sets the font and property for all pages/Sections And others
        /// </summary>
        /// <param name="fnt">Font Choice</param>
        internal void SetFontValue(SetFont fnt)
        {
            Font = fnt;
            switch (Font)
            {
                case SetFont.MSSANS10:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    break;
                case SetFont.MSSANS11:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 11, FontStyle.Regular);
                    break;
                case SetFont.MSSANS12:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    break;
                case SetFont.MSSANS14:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 14, FontStyle.Regular);
                    break;
                case SetFont.MSSANS10B:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                    break;
                case SetFont.MSSANS11B:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 11, FontStyle.Bold);
                    break;
                case SetFont.MSSANS12B:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    break;
                case SetFont.MSSANS14B:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                    break;
                default:
                    GetFont = new System.Drawing.Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    break;
            }
        }
        /// <summary>
        /// Font Choices
        /// </summary>
        internal enum SetFont
        {
            /// <summary>
            /// Microsoft Sans Serif, 10pt, Regular
            /// </summary>
            MSSANS10 = 0,
            /// <summary>
            /// Microsoft Sans Serif, 11pt, Regular
            /// </summary>
            MSSANS11 = 1,
            /// <summary>
            /// Microsoft Sans Serif, 12pt, Regular
            /// </summary>
            MSSANS12 = 3,
            /// <summary>
            /// Microsoft Sans Serif, 14pt, Regular
            /// </summary>
            MSSANS14 = 4,
            /// <summary>
            /// Microsoft Sans Serif, 10pt, Bold
            /// </summary>
            MSSANS10B = 5,
            /// <summary>
            /// Microsoft Sans Serif, 11pt, Bold
            /// </summary>
            MSSANS11B = 6,
            /// <summary>
            /// Microsoft Sans Serif, 12pt, Bold
            /// </summary>
            MSSANS12B = 7,
            /// <summary>
            /// Microsoft Sans Serif, 14pt, Bold
            /// </summary>
            MSSANS14B = 8
        }
    }
}
