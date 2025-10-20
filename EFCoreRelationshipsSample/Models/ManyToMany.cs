using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EFCoreRelationshipsSample.Models;

[DebuggerDisplay($"Course: {{{nameof(CourseTitle)}}}, Credits: {{{nameof(Credits)}}}")]
public class Course
{
    [Key]
    public int Id { get; set; }

    public required string CourseTitle { get; set; }

    public byte Credits { get; set; }

    public ICollection<Student> Students { get; set; } = [];
}

[DebuggerDisplay($"Student: {{{nameof(FullName)}}}")]
public class Student
{
    [Key]
    public int Id { get; set; }

    public required string FullName { get; set; }

    public ICollection<Course> Courses { get; set; } = [];
}