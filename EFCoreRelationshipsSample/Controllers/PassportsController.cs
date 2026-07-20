using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCoreRelationshipsSample.Data;
using EFCoreRelationshipsSample.Models;

namespace EFCoreRelationshipsSample.Controllers
{
    // Dependent side of the one-to-one example. Because PersonId has a unique index,
    // a person can be linked to at most one passport, so the Person dropdown only
    // offers people who do not already have one.
    public class PassportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Passports
        public async Task<IActionResult> Index()
        {
            var passports = _context.Passports.Include(p => p.Person);
            return View(await passports.ToListAsync());
        }

        // GET: Passports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passport = await _context.Passports
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passport == null)
            {
                return NotFound();
            }

            return View(passport);
        }

        // GET: Passports/Create
        public IActionResult Create()
        {
            PopulatePeopleDropDownList();
            return View();
        }

        // POST: Passports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PassportNumber,IssueDate,PersonId")] Passport passport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(passport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulatePeopleDropDownList(passport.PersonId);
            return View(passport);
        }

        // GET: Passports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passport = await _context.Passports.FindAsync(id);
            if (passport == null)
            {
                return NotFound();
            }
            PopulatePeopleDropDownList(passport.PersonId);
            return View(passport);
        }

        // POST: Passports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PassportNumber,IssueDate,PersonId")] Passport passport)
        {
            if (id != passport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(passport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PassportExists(passport.Id))
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
            PopulatePeopleDropDownList(passport.PersonId);
            return View(passport);
        }

        // GET: Passports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var passport = await _context.Passports
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (passport == null)
            {
                return NotFound();
            }

            return View(passport);
        }

        // POST: Passports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passport = await _context.Passports.FindAsync(id);
            if (passport != null)
            {
                _context.Passports.Remove(passport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PassportExists(int id)
        {
            return _context.Passports.Any(e => e.Id == id);
        }

        // Only people without a passport can be assigned a new one (unique one-to-one FK).
        // The optionally supplied personId keeps the currently linked person selectable on Edit.
        private void PopulatePeopleDropDownList(int? selectedPerson = null)
        {
            var availablePeople = _context.People
                .Where(p => p.Passport == null || p.Id == selectedPerson)
                .OrderBy(p => p.FullName);

            ViewBag.PersonId = new SelectList(availablePeople, "Id", "FullName", selectedPerson);
        }
    }
}
