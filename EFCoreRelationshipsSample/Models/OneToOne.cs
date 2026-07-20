using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EFCoreRelationshipsSample.Models;

[DebuggerDisplay($"Person: {{{nameof(FullName)}}}")]
public class Person
{
    [Key]
    public int Id { get; set; }

    public required string FullName { get; set; }

    // Principal side of the one-to-one relationship: a person has (at most) one passport.
    public Passport? Passport { get; set; }
}

[DebuggerDisplay($"Passport: {{{nameof(PassportNumber)}}}")]
public class Passport
{
    [Key]
    public int Id { get; set; }

    public required string PassportNumber { get; set; }

    [DataType(DataType.Date)]
    public DateOnly IssueDate { get; set; }

    // Dependent side of the one-to-one relationship. The unique index EF Core adds
    // on this foreign key is what constrains each person to a single passport.
    public int PersonId { get; set; }

    public Person? Person { get; set; }
}
