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
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (course == null)
            {
                return NotFound();
            }

            // Get all students not enrolled in this course
            var enrolledStudentIds = course.Students.Select(s => s.Id).ToList();
            var availableStudents = await _context.Students
                .Where(s => !enrolledStudentIds.Contains(s.Id))
                .ToListAsync();

            var viewModel = new CourseDetailsViewModel
            {
                Id = course.Id,
                CourseTitle = course.CourseTitle,
                Credits = course.Credits,
                EnrolledStudents = course.Students.ToList(),
                AvailableStudents = availableStudents
            };

            return View(viewModel);
        }

        // POST: Courses/AddStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(int courseId, int studentId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound();
            }

            // Check if student is already enrolled
            if (!course.Students.Any(s => s.Id == studentId))
            {
                course.Students.Add(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // POST: Courses/WithdrawStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WithdrawStudent(int courseId, int studentId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var student = course.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                course.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = courseId });
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseTitle,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseTitle,Credits")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
