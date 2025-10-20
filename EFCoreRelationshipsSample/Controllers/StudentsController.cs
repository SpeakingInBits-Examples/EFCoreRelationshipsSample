using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFCoreRelationshipsSample.Data;
using EFCoreRelationshipsSample.Models;

namespace EFCoreRelationshipsSample.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get selected student and their current course list
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);
            
            if (student == null)
            {
                return NotFound();
            }

            List<Course> allCourses = await _context.Courses.ToListAsync();

            StudentEditViewModel viewModel = new()
            {
                Id = student.Id,
                FullName = student.FullName,
                SelectedCourseIds = student.Courses.Select(c => c.Id).ToList(),
                AvailableCourses = allCourses,
                EnrolledCourses = student.Courses.ToList()
            };

            return View(viewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var student = await _context.Students
                        .Include(s => s.Courses)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    if (student == null)
                    {
                        return NotFound();
                    }

                    // Update student properties
                    student.FullName = viewModel.FullName;

                    // Clear existing courses
                    student.Courses.Clear();

                    // Add selected courses
                    if (viewModel.SelectedCourseIds != null && viewModel.SelectedCourseIds.Any())
                    {
                        var selectedCourses = await _context.Courses
                            .Where(c => viewModel.SelectedCourseIds.Contains(c.Id))
                            .ToListAsync();

                        foreach (var course in selectedCourses)
                        {
                            student.Courses.Add(course);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(viewModel.Id))
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

            // If we got this far, ModelState was invalid, reload the view
            viewModel.AvailableCourses = await _context.Courses.ToListAsync();
            viewModel.EnrolledCourses = await _context.Courses
                .Where(c => viewModel.SelectedCourseIds.Contains(c.Id))
                .ToListAsync();
            
            return View(viewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
