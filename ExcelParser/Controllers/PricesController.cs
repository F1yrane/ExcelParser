using ExcelParser.Data;
using ExcelParser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcelParser.Controllers
{
    public class PricesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Prices
        public async Task<IActionResult> Index()
        {
            var moveDBs = await _context.MoveDBs.ToListAsync();
            var prices = await _context.Prices.ToListAsync();
            var model = new ViewModel { Prices = prices, MoveDBs = moveDBs };
            return View(model);
        }

        // GET: Prices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // GET: Prices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CodeRab,Name,Cost")] Price price)
        {
            if (ModelState.IsValid)
            {
                _context.Add(price);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        // GET: Prices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }
            return View(price);
        }

        // POST: Prices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CodeRab,Name,Cost")] Price price)
        {
            if (id != price.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(price);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.Id))
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
            return View(price);
        }

        // GET: Prices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        // POST: Prices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceExists(int id)
        {
            return _context.Prices.Any(e => e.Id == id);
        }

        // GET: MoveDBs/Delete/5
        public async Task<IActionResult> DeleteMoveDB(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moveDB = await _context.MoveDBs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moveDB == null)
            {
                return NotFound();
            }

            return View(moveDB);
        }

        // POST: MoveDBs/Delete/5
        [HttpPost, ActionName("DeleteMoveDB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMoveDBConfirmed(int id)
        {
            var moveDB = await _context.MoveDBs.FindAsync(id);
            _context.MoveDBs.Remove(moveDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoveDBExists(int id)
        {
            return _context.MoveDBs.Any(e => e.Id == id);
        }

        // GET: MoveDBs/Edit/5
        public async Task<IActionResult> EditMoveDB(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moveDB = await _context.MoveDBs.FindAsync(id);
            if (moveDB == null)
            {
                return NotFound();
            }
            return View(moveDB);
        }

        // POST: MoveDBs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMoveDB(int id, [Bind("Id,CodeRem,CodeRab,Name")] MoveDB moveDB)
        {
            if (id != moveDB.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moveDB);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoveDBExists(moveDB.Id))
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
            return View(moveDB);
        }

        // GET: MoveDBs/Create
        public IActionResult CreateMoveDB()
        {
            return View();
        }

        // POST: MoveDBs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMoveDB([Bind("Id,CodeRem,CodeRab,Name")] MoveDB moveDB)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moveDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(moveDB);
        }
    }
}
