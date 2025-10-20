using EFCoreRelationshipsSample.Models;
using EFCoreRelationshipsSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationshipsSample.Controllers;

public class CourseDashboardController : Controller
{
    private readonly ICourseService _courseService;

    public CourseDashboardController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> CourseRosters()
    {
        var viewModel = new CourseDashboardViewModel
        {
            Courses = await _courseService.GetCoursesWithStudentsAsync(),
            UnenrolledStudents = await _courseService.GetUnenrolledStudentsAsync(),
            EmptyCourses = await _courseService.GetEmptyCoursesAsync()
        };

        return View(viewModel);
    }
}