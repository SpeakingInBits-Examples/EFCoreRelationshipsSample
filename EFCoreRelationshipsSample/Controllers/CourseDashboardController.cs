using EFCoreRelationshipsSample.Data;
using EFCoreRelationshipsSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsSample.Controllers;

public class CourseDashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public CourseDashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> CourseRosters()
    {
        // Get a list of courses that have enrolled students
        List<Course> courses = await _context.Courses
            .Include(course => course.Students)
            .Where(course => course.Students.Any())
            .ToListAsync();

        // Students not enrolled in any course
        List<Student> unenrolledStudents = await _context.Students
            .Where(student => !student.Courses.Any())
            .ToListAsync();

        // Courses with no students enrolled
        List<Course> emptyCourses = await _context.Courses
            .Where(course => !course.Students.Any())
            .ToListAsync();

        CourseDashboardViewModel viewModel = new()
        {
            Courses = courses,
            UnenrolledStudents = unenrolledStudents,
            EmptyCourses = emptyCourses
        };

        return View(viewModel);
    }
}

public class CourseDashboardViewModel
{
    /// <summary>
    /// List of courses and their associated students
    /// </summary>
    public ICollection<Course> Courses { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of students who are not currently enrolled in any course.
    /// </summary>
    public ICollection<Student> UnenrolledStudents { get; set; } = [];

    /// <summary>
    /// Gets or sets the collection of courses that currently have no enrolled students.
    /// </summary>
    public ICollection<Course> EmptyCourses { get; set; } = [];
}