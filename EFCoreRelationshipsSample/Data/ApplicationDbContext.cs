using EFCoreRelationshipsSample.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsSample.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public DbSet<FoodItem> FoodItems { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }
}
