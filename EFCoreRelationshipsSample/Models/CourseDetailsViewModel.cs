namespace EFCoreRelationshipsSample.Models;

public class CourseDetailsViewModel
{
    public int Id { get; set; }
    public required string CourseTitle { get; set; }
    public byte Credits { get; set; }
    
    // Students currently enrolled in this course
    public List<Student> EnrolledStudents { get; set; } = [];
    
    // Students available to add (not currently enrolled)
    public List<Student> AvailableStudents { get; set; } = [];
}
