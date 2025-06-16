namespace MyntraClone.API.DTOs
{
    /// <summary>
    /// DTO for representing category hierarchy with nested subcategories
    /// </summary>
    public class CategoryHierarchyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public int ProductCount { get; set; }
        public List<CategoryHierarchyDto> SubCategories { get; set; } = new List<CategoryHierarchyDto>();
        public bool HasSubCategories => SubCategories.Any();
        public int Level { get; set; } = 0; // For UI indentation purposes
    }
}
