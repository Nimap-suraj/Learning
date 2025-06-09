using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using System.Xml.Linq;

namespace ExportData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        [HttpGet("excel")]
        //[Obsolete]
        public IActionResult ExportToExcel()
        {
            var user = new List<UserInfo>
            {
               new UserInfo {Id = 1,Name="suraj shah",Age=12 },
               new UserInfo {Id = 2,Name="om sambhar",Age=12 },
               new UserInfo {Id = 3,Name="rajat pandit",Age=12 }
            };
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // New way in EPPlus 8+
            //ExcelPackage.License = new OfficeOpenXml.License.LicenseProvider
            //{
            //    LicenseContext = LicenseContext.NonCommercial
            //};

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Users");
            worksheet.Cells[1, 1].Value = "Id";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Age";

            for (int i = 0;i< user.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = user[i].Id;
                worksheet.Cells[i + 2, 2].Value = user[i].Name;
                worksheet.Cells[i + 2, 3].Value = user[i].Age;
            }

            var excelData = package.GetAsByteArray();
            // Return as a downloadable file
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "People.xlsx");
        }
    }
}
