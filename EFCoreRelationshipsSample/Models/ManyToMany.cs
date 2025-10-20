using System.ComponentModel.DataAnnotations;

namespace EFCoreRelationshipsSample.Models;

public class Course
{
    [Key]
    public int Id { get; set; }

    public required string CourseTitle { get; set; }

    public byte Credits { get; set; }

    public ICollection<Student> Students { get; set; } = [];
}

public class Student
{
    [Key]
    public int Id { get; set; }

    public required string FullName { get; set; }

    public ICollection<Course> Courses { get; set; } = [];
}