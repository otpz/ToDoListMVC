using Microsoft.AspNetCore.Http;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Security.Claims;
using ToDoListMVC.Entity.Entities;
using ToDoListMVC.Service.Extensions;
using ToDoListMVC.Service.Services.Abstractions;

namespace ToDoListMVC.Service.Helpers.PdfGenerator
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public PdfGenerator(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
        }

        public async Task<string> GenerateUserDataPdfById(int userId)
        {
            if (userId != _user.GetLoggedInUserId())
            {
                return null;
            }

            var user = await userService.GetUserProfileWithTaskByIdAsync(userId);
            
            PdfDocument pdfDoc = new PdfDocument();
            PdfPage page = pdfDoc.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont titleFont = new XFont("Verdana", 14, XFontStyle.Bold);
            XFont footerFont = new XFont("Verdana", 10, XFontStyle.Regular);
            XFont dateFont = new XFont("Verdana", 10, XFontStyle.Regular);
            XFont tableFont = new XFont("Verdana", 10, XFontStyle.Regular);

            XBrush lightBlueBrush = new XSolidBrush(XColor.FromArgb(173, 216, 230)); // Açık mavi renk

            XPoint activeTasksDotPosition = new XPoint(200, 64);
            
            // Header
            gfx.DrawString("Developed By Osman Topuz.", dateFont, XBrushes.Black,
                new XRect(10, 30, page.Width, page.Height),
                new XStringFormat { Alignment = XStringAlignment.Near });

            gfx.DrawString("Oluşturulma Tarihi: " + DateTime.Now.ToString("F"), dateFont, XBrushes.Black,
                new XRect(-10, 30, page.Width, page.Height),
                new XStringFormat { Alignment = XStringAlignment.Far });

            gfx.DrawEllipse(XBrushes.Green, new XRect(activeTasksDotPosition.X, activeTasksDotPosition.Y, 10, 10));
            gfx.DrawString("Aktif Görevlerim", titleFont, XBrushes.Black,
                new XRect(0, 60, page.Width, page.Height),
                new XStringFormat { Alignment = XStringAlignment.Center });

            // Table
            int startY = 100; // Starting Y position for the table
            int rowHeight = 20; // Height of each row
            int tableWidth = (int)page.Width - 40; // Width of the table

            // Column Widths
            int[] columnWidths = { tableWidth / 4, tableWidth / 4, tableWidth / 4, tableWidth / 4 };

            // Table Headers
            gfx.DrawRectangle(XPens.Black, lightBlueBrush, new XRect(20, startY, tableWidth, rowHeight));
            gfx.DrawString("Görev Adı", tableFont, XBrushes.Black, new XRect(20, startY, columnWidths[0], rowHeight), XStringFormats.Center);
            gfx.DrawString("Açıklama", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0], startY, columnWidths[1], rowHeight), XStringFormats.Center);
            gfx.DrawString("Önem", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1], startY, columnWidths[2], rowHeight), XStringFormats.Center);
            gfx.DrawString("Tarih", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, columnWidths[3], rowHeight), XStringFormats.Center);

            // Draw vertical lines for columns
            gfx.DrawLine(XPens.Black, 20 + columnWidths[0], startY, 20 + columnWidths[0], startY + rowHeight * (user.TaskJobs.Count + 1));
            gfx.DrawLine(XPens.Black, 20 + columnWidths[0] + columnWidths[1], startY, 20 + columnWidths[0] + columnWidths[1], startY + rowHeight * (user.TaskJobs.Count + 1));
            gfx.DrawLine(XPens.Black, 20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, 20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY + rowHeight * (user.TaskJobs.Count + 1));

            // Table Rows for Active Tasks
            foreach (var taskJob in user.TaskJobs)
            {
                startY += rowHeight;
                gfx.DrawRectangle(XPens.Black, new XRect(20, startY, tableWidth, rowHeight));
                gfx.DrawString(taskJob.Title, tableFont, XBrushes.Black, new XRect(20, startY, columnWidths[0], rowHeight), XStringFormats.Center);
                gfx.DrawString(taskJob.Description, tableFont, XBrushes.Black, new XRect(20 + columnWidths[0], startY, columnWidths[1], rowHeight), XStringFormats.Center);
                gfx.DrawString(PriorityDetector(taskJob.Priority), tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1], startY, columnWidths[2], rowHeight), XStringFormats.Center);
                gfx.DrawString(taskJob.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"), tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, columnWidths[3], rowHeight), XStringFormats.Center);
            }

            var userDisabledTask = await userService.GetUserProfileWithDisabledTaskByIdAsync(userId);
            userDisabledTask.TaskJobs = userDisabledTask.TaskJobs.Where(x => !x.IsActive && !x.IsDeleted).ToList();

            // If there are completed tasks, add a new section for them
            if (userDisabledTask.TaskJobs.Count > 0)
            {
                startY += rowHeight + 40; // Adding extra space between tables
                XPoint completedTasksDotPosition = new XPoint(165, startY+5);
                gfx.DrawEllipse(XBrushes.Red, new XRect(completedTasksDotPosition.X, completedTasksDotPosition.Y, 10, 10));
                gfx.DrawString("Tamamlanmış Görevlerim", titleFont, XBrushes.Black,
                    new XRect(0, startY, page.Width, page.Height),
                    new XStringFormat { Alignment = XStringAlignment.Center });

                startY += 40; // Move down for the new table header

                // Table Headers for Completed Tasks
                gfx.DrawRectangle(XPens.Black, lightBlueBrush, new XRect(20, startY, tableWidth, rowHeight));
                gfx.DrawString("Görev Adı", tableFont, XBrushes.Black, new XRect(20, startY, columnWidths[0], rowHeight), XStringFormats.Center);
                gfx.DrawString("Açıklama", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0], startY, columnWidths[1], rowHeight), XStringFormats.Center);
                gfx.DrawString("Önem", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1], startY, columnWidths[2], rowHeight), XStringFormats.Center);
                gfx.DrawString("Tarih", tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, columnWidths[3], rowHeight), XStringFormats.Center);

                // Draw vertical lines for columns for completed tasks
                gfx.DrawLine(XPens.Black, 20 + columnWidths[0], startY, 20 + columnWidths[0], startY + rowHeight * (userDisabledTask.TaskJobs.Count + 1));
                gfx.DrawLine(XPens.Black, 20 + columnWidths[0] + columnWidths[1], startY, 20 + columnWidths[0] + columnWidths[1], startY + rowHeight * (userDisabledTask.TaskJobs.Count + 1));
                gfx.DrawLine(XPens.Black, 20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, 20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY + rowHeight * (userDisabledTask.TaskJobs.Count + 1));

                // Table Rows for Completed Tasks
                foreach (var taskjob in userDisabledTask.TaskJobs)
                {
                    startY += rowHeight;
                    gfx.DrawRectangle(XPens.Black, new XRect(20, startY, tableWidth, rowHeight));
                    gfx.DrawString(taskjob.Title, tableFont, XBrushes.Black, new XRect(20, startY, columnWidths[0], rowHeight), XStringFormats.Center);
                    gfx.DrawString(taskjob.Description, tableFont, XBrushes.Black, new XRect(20 + columnWidths[0], startY, columnWidths[1], rowHeight), XStringFormats.Center);
                    gfx.DrawString(PriorityDetector(taskjob.Priority), tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1], startY, columnWidths[2], rowHeight), XStringFormats.Center);
                    gfx.DrawString(taskjob.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"), tableFont, XBrushes.Black, new XRect(20 + columnWidths[0] + columnWidths[1] + columnWidths[2], startY, columnWidths[3], rowHeight), XStringFormats.Center);
                }
            }

            gfx.DrawString("www.github.com/otpz", footerFont, XBrushes.Black,
                new XRect(0, startY+100, page.Width, page.Height),
                new XStringFormat { Alignment = XStringAlignment.Center });

            // Save the document
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // desktop path
            pdfDoc.Save(path+"\\"+$"Tasks_{user.FirstName}_{user.LastName}.pdf");

            return "Success";
        }

        private string PriorityDetector(int priority) 
        {
            if (priority == 1) 
            {
                return "Yüksek Öncelikli";
            } 
            else if (priority == 2)
            {
                return "Orta Öncelikli";
            } 
            else
            {
                return "Düşük Öncelikli";
            }
        }


    }
}
