using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExcelParser.Models;
using ExcelParser.Data;
using System.IO;
using NPOI.HSSF.UserModel; 
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel; 
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace Parse3.Controllers
{
    public partial class HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment, ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();
            var customers = _context.Customers.ToList();
            var costRems = _context.CostRems.ToList();
            var moveDBs = _context.MoveDBs.ToList();
            var prices = _context.Prices.ToList();
            var model = new ViewModel
            {
                Orders = orders,
                Customers = customers,
                CostRems = costRems,
                Prices = prices,
                MoveDBs = moveDBs
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Import()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                file.CopyTo(stream);
                stream.Position = 0;

                if (fileExtension == ".xls")
                {
                    var hssfWorkbook = new HSSFWorkbook(stream); // Excel 97-2000
                    sheet = hssfWorkbook.GetSheetAt(0); // Get first sheet from workbook
                }
                else
                {
                    var xssfWorkbook = new XSSFWorkbook(stream); // Excel 2007+
                    sheet = xssfWorkbook.GetSheetAt(0); // Get first sheet from workbook
                }

                IRow headerRow = sheet.GetRow(0); // Get Header Row
                int cellCount = headerRow.LastCellNum;
                sb.Append("<table class='table table-bordered'><tr>");

                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                    {
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                }

                sb.Append("</tr>");

                // Удаление всех данных из таблиц
                _context.Orders.RemoveRange(_context.Orders);
                _context.CodeRems.RemoveRange(_context.CodeRems);
                _context.CostRems.RemoveRange(_context.CostRems);
                _context.Customers.RemoveRange(_context.Customers);
                _context.SaveChanges();

                int orderNumber = 1;

                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) // Read Excel File
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null || row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                    sb.AppendLine("<tr>");
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                    }
                    sb.AppendLine("</tr>");

                    string rawCodes = row.GetCell(4).ToString().Trim();
                    MatchCollection matchedCodes = MatchedCodes().Matches(rawCodes);
                    string? date = row.GetCell(2).ToString();
                    string? customerName = row.GetCell(1).ToString();

                    var orders = new Order[]
                    {
                            new() { Num = orderNumber, Name = "Заказ" + orderNumber, Date = date, NameCustomers = customerName }
                    };

                    _context.Orders.AddRange(orders);
                    _context.SaveChanges();

                    orderNumber++;
                }

                var moveDBs = _context.MoveDBs.ToList();
                var prices = _context.Prices.ToList();
                var ordersList = _context.Orders.ToList();

                foreach (var moveDB in moveDBs)
                {
                    foreach (var price in prices)
                    {
                        if (moveDB.Coderab == price.Coderab)
                        {
                            var costRems = new CostRem[]
                            {
                                    new() { Coderab = moveDB.Coderab, Name = price.Name, Cost = price.Cost, Nid = moveDB.Name }
                            };

                            _context.CostRems.AddRange(costRems);
                        }
                    }
                }
                _context.SaveChanges();

                int totalCost = 0;
                var costRemsList = _context.CostRems.ToList();

                foreach (var order in ordersList)
                {
                    foreach (var costRem in costRemsList)
                    {
                        if (order.Num == costRem.Nid)
                        {
                            totalCost += costRem.Cost;
                        }
                    }

                    var customers = new Customer[]
                    {
                            new() { Nid1 = order.Num, Date = order.Date, Name = order.NameCustomers, EndCost = totalCost }
                    };

                    _context.Customers.AddRange(customers);
                    totalCost = 0;
                }
                _context.SaveChanges();
            }

            return this.Content(sb.ToString());
        }

        [GeneratedRegex(@"\d+\.\d+")]
        private static partial Regex MatchedCodes();
    }
}
