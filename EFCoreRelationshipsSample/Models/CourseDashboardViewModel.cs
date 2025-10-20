namespace EFCoreRelationshipsSample.Models;

public class CourseDashboardViewModel
{
    /// <summary>
    /// List of courses and their associated students. This property should
    /// only contain courses that have at least 1 student. NO EMPTY COURSES
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
