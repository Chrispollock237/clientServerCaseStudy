using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Reports
{
    public class CallReport
    {
        static Font catFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
        static Font smallFont = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
        static Font cellFont = new Font(Font.FontFamily.COURIER, 11, Font.NORMAL);
        static string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        static string IMG = "img/digitalworld - headerSize.jpg";

        public void doIt()
        {
            try
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(mappedPath + "Pdfs/Calls.pdf", FileMode.Create));
                document.Open();
                Paragraph para = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);
                image1.SetAbsolutePosition(245f, 740f);
                image1.ScaleAbsoluteHeight(90f);
                image1.ScaleAbsoluteWidth(120f);
                para.Add(image1);
                para.Alignment = Element.ALIGN_RIGHT;

                // Lets write a big header
                addEmptyLine(para, 3);
                Paragraph mainHead = new Paragraph(String.Format("{0,8}", "Calls"), catFont);
                mainHead.Alignment = Element.ALIGN_CENTER;
                para.Add(mainHead);
                addEmptyLine(para, 1);

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 96.00F;
                float[] widths = new float[] { 16f, 16f, 16f, 18f, 30f, 40f };
                table.SetWidths(widths);
                table.AddCell(addCell("Date Opened", "h"));
                table.AddCell(addCell("Date Closed", "h"));
                table.AddCell(addCell("Employee", "h"));
                table.AddCell(addCell("Technician", "h"));
                table.AddCell(addCell("Problem", "h"));
                table.AddCell(addCell("Notes", "h"));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                CallViewModel call = new CallViewModel();
                List<CallViewModel> calls = call.GetAll();

                foreach (CallViewModel callVm in calls)
                {
                    table.AddCell(addCell(callVm.DateOpened.ToString()));
                    table.AddCell(addCell(callVm.DateClosed.ToString()));
                    table.AddCell(addCell(callVm.EmployeeName));
                    table.AddCell(addCell(callVm.TechName));
                    table.AddCell(addCell(callVm.ProblemDescription));
                    table.AddCell(addCell(callVm.Notes));
                }

                para.Add(table);
                addEmptyLine(para, 3);
                para.Alignment = Element.ALIGN_CENTER;
                Paragraph footer = new Paragraph("Call report written on - " + DateTime.Now, smallFont);
                footer.Alignment = Element.ALIGN_CENTER;
                para.Add(footer);
                document.Add(para);
                document.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
            }
        }

        public static void addEmptyLine(Paragraph paragraph, int number)
        {
            for (int i = 0; i < number; i++)
            {
                paragraph.Add(new Paragraph(" "));
            }
        }

        private PdfPCell addCell(string data, string celltype = "d")
        {
            PdfPCell cell;

            if (celltype == "h")
            {
                cell = new PdfPCell(new Phrase(data, smallFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = Rectangle.NO_BORDER;
            }
            else
            {
                cell = new PdfPCell(new Phrase(data, cellFont));
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = Rectangle.NO_BORDER;
            }
            return cell;
        }
    }
}