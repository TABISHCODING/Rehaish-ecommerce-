namespace MyntraClone.API.Models;

public class Category
{
    public int Id { get; set; }

    public required string Name { get; set; } = "";
    public required string Description { get; set; } = "";

    // Hierarchical properties
    public int? ParentCategoryId { get; set; }
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
