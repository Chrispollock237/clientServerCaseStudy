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
    public class EmployeeReport
    {
        static Font catFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
        static Font smallFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD);
        static Font cellFont = new Font(Font.FontFamily.COURIER, 11, Font.NORMAL);
        static string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        static string IMG = "img/digitalworld - headerSize.jpg";

        public void doIt()
        {
            try
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(mappedPath + "Pdfs/Employees.pdf", FileMode.Create));
                document.Open();
                Paragraph para = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);
                image1.SetAbsolutePosition(245f, 740f);
                image1.ScaleAbsoluteHeight(90f);
                image1.ScaleAbsoluteWidth(120f);
                para.Add(image1);
                para.Alignment = Element.ALIGN_CENTER;

                // Lets write a big header
                addEmptyLine(para, 3);
                Paragraph mainHead = new Paragraph(String.Format("{0,8}", "Employees"), catFont);
                mainHead.Alignment = Element.ALIGN_CENTER;
                para.Add(mainHead);
                addEmptyLine(para, 1);

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 96.00F;
                float[] widths = new float[] { 8f, 16f, 23f, 27f, 28f, 28f };
                table.SetWidths(widths);
                table.AddCell(addCell("Title", "h"));
                table.AddCell(addCell("First Name", "h"));
                table.AddCell(addCell("Surname", "h"));
                table.AddCell(addCell("Phone #", "h"));
                table.AddCell(addCell("Email", "h"));
                table.AddCell(addCell("Department", "h"));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                table.AddCell(addCell(" "));
                EmployeeViewModel employee = new EmployeeViewModel();
                List<EmployeeViewModel> employees = employee.GetAll();

                foreach (EmployeeViewModel emp in employees)
                {
                    table.AddCell(addCell(emp.Title));
                    table.AddCell(addCell(emp.Firstname));
                    table.AddCell(addCell(emp.Lastname));
                    table.AddCell(addCell(emp.Phoneno));
                    table.AddCell(addCell(emp.Email));
                    table.AddCell(addCell(emp.DepartmentName));
                }

                para.Add(table);
                addEmptyLine(para, 3);
                para.Alignment = Element.ALIGN_CENTER;
                Paragraph footer = new Paragraph("Employee report written on - " + DateTime.Now, smallFont);
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