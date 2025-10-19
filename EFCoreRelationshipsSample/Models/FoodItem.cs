using System.ComponentModel.DataAnnotations;

namespace EFCoreRelationshipsSample.Models;

public class FoodItem
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
}
