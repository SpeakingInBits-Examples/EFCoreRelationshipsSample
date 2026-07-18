using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EFCoreRelationshipsSample.Models;

[DebuggerDisplay($"Blog: {{{nameof(Name)}}}")]
public class Blog
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    [DataType(DataType.Url)]
    public string? Url { get; set; }

    // Principal (one) side of the one-to-many relationship: a blog has many posts.
    public ICollection<Post> Posts { get; set; } = [];
}

[DebuggerDisplay($"Post: {{{nameof(Title)}}}")]
public class Post
{
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }

    public string? Content { get; set; }

    // Dependent (many) side of the one-to-many relationship. By convention EF Core
    // uses the BlogId foreign key to associate each post with a single blog.
    public int BlogId { get; set; }

    public Blog? Blog { get; set; }
}
