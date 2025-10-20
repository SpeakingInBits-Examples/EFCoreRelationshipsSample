namespace EFCoreRelationshipsSample.Models;

public class StudentEditViewModel
{
    /// <summary>
    /// Id of the student that is being edited
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the student that is being edited
    /// </summary>
    public required string FullName { get; set; }
    
    /// <summary>
    /// List of course IDs that the student is currently enrolled in
    /// </summary>
    public List<int> SelectedCourseIds { get; set; } = [];
    
    /// <summary>
    /// List of all available courses for the dropdown
    /// </summary>
    public List<Course> AvailableCourses { get; set; } = [];
    
    /// <summary>
    /// List of courses the student is currently enrolled in (for display)
    /// </summary>
    public List<Course> EnrolledCourses { get; set; } = [];
}
