using EFCoreRelationshipsSample.Data;
using EFCoreRelationshipsSample.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsSample.Services;

public interface ICourseService
{
    Task<List<Course>> GetCoursesWithStudentsAsync();
    Task<List<Student>> GetUnenrolledStudentsAsync();
    Task<List<Course>> GetEmptyCoursesAsync();
}

public class CourseService : ICourseService
{
    private readonly ApplicationDbContext _context;

    public CourseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetCoursesWithStudentsAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Include(course => course.Students)
            .Where(course => course.Students.Any())
            .ToListAsync();
    }

    public async Task<List<Student>> GetUnenrolledStudentsAsync()
    {
        return await _context.Students
            .AsNoTracking()
            .Where(student => !student.Courses.Any())
            .ToListAsync();
    }

    public async Task<List<Course>> GetEmptyCoursesAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .Where(course => !course.Students.Any())
            .ToListAsync();
    }
}
