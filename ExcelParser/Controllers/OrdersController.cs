using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExcelParser.Models;
using ExcelParser.Data;
using System.Linq;
using System.Threading.Tasks;


namespace ExcelParser.Controllers
{
    public class OrdersController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.ToListAsync();
            var customers = await _context.Customers.ToListAsync();
            var costRems = await _context.CostRems.ToListAsync();
            var moveDBs = await _context.MoveDBs.ToListAsync();
            var prices = await _context.Prices.ToListAsync();

            var model = new ViewModel
            {
                Orders = orders,
                Customers = customers,
                CostRems = costRems,
                MoveDBs = moveDBs,
                Prices = prices
            };

            return View(model);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.ToListAsync();
            var customers = await _context.Customers.ToListAsync();
            var costRems = await _context.CostRems.ToListAsync();
            var moveDBs = await _context.MoveDBs.ToListAsync();
            var prices = await _context.Prices.ToListAsync();

            var model = new ViewModel
            {
                Orders = orders,
                Customers = customers,
                CostRems = costRems,
                MoveDBs = moveDBs,
                Prices = prices
            };

            return View(model);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Num,Name,Date,NameCustomers")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Num,Name,Date,NameCustomers")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _ = _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
