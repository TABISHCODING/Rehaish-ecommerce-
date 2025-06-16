// File: Services/CategoryService.cs
using Microsoft.EntityFrameworkCore; // For async DB operations
using MyntraClone.API.Data;           // For ApplicationDbContext
using MyntraClone.API.DTOs;           // For DTOs
using MyntraClone.API.Models;         // For Category model

namespace MyntraClone.API.Services
{
    public class CategoryService : ICategoryService // Implements the ICategoryService interface
    {
        private readonly ApplicationDbContext _context; // EF Core DbContext to interact with the database

        public CategoryService(ApplicationDbContext context)
        {
            _context = context; // Injecting the ApplicationDbContext
        }

        // Method to get all categories with optional name filtering and pagination
        public async Task<IEnumerable<CategoryDto>> GetAllAsync(string? name, int page, int pageSize)
        {
            var query = _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .AsQueryable(); // Start the query to fetch categories

            // If a name is provided, filter the categories by name (case-insensitive search)
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name)); // Filter by name
            }

            // Fetch categories with pagination
            var categories = await query
                .Skip((page - 1) * pageSize)  // Skip to the correct page
                .Take(pageSize)               // Limit the number of categories to the page size
                .Select(c => new CategoryDto  // Project each category to a CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                    ProductCount = c.Products.Count,
                    HasSubCategories = c.SubCategories.Any()
                })
                .ToListAsync(); // Execute the query and return the result as a list

            return categories; // Return the list of categories
        }

        // Method to get a category by its ID
        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                    ProductCount = c.Products.Count,
                    HasSubCategories = c.SubCategories.Any()
                })
                .FirstOrDefaultAsync(); // Return the first matching category or null if not found
        }

        // Method to create a new category
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            // Validate parent category exists if provided
            if (dto.ParentCategoryId.HasValue)
            {
                var parentExists = await _context.Categories.AnyAsync(c => c.Id == dto.ParentCategoryId.Value);
                if (!parentExists)
                {
                    throw new ArgumentException("Parent category does not exist");
                }
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId
            };

            _context.Categories.Add(category); // Add the new category to the DbContext
            await _context.SaveChangesAsync(); // Save the changes to the database

            // Reload with parent information
            var createdCategory = await GetByIdAsync(category.Id);
            return createdCategory!; // Return the newly created category as a DTO
        }

        // Method to update an existing category by ID
        public async Task<bool> UpdateAsync(int id, CreateCategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id); // Find the category by ID
            if (category == null)
                return false; // If category not found, return false

            // Validate parent category exists if provided and prevent circular reference
            if (dto.ParentCategoryId.HasValue)
            {
                if (dto.ParentCategoryId.Value == id)
                {
                    throw new ArgumentException("Category cannot be its own parent");
                }

                var parentExists = await _context.Categories.AnyAsync(c => c.Id == dto.ParentCategoryId.Value);
                if (!parentExists)
                {
                    throw new ArgumentException("Parent category does not exist");
                }

                // Check for circular reference (prevent setting a descendant as parent)
                if (await IsDescendantAsync(id, dto.ParentCategoryId.Value))
                {
                    throw new ArgumentException("Cannot set a descendant category as parent");
                }
            }

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.ParentCategoryId = dto.ParentCategoryId; // Update the category fields

            await _context.SaveChangesAsync(); // Save the changes to the database
            return true; // Return true to indicate the update was successful
        }

        // Method to delete a category by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id); // Find the category by ID

            if (category == null)
                return false; // If category not found, return false

            // Check if category has subcategories
            if (category.SubCategories.Any())
            {
                throw new InvalidOperationException("Cannot delete category that has subcategories. Delete subcategories first.");
            }

            _context.Categories.Remove(category); // Remove the category from the DbContext
            await _context.SaveChangesAsync(); // Save the changes to the database
            return true; // Return true to indicate the delete was successful
        }

        // Get full category hierarchy as a tree structure
        public async Task<IEnumerable<CategoryHierarchyDto>> GetHierarchyAsync()
        {
            var allCategories = await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.Products)
                .ToListAsync();

            // Get root categories (no parent)
            var rootCategories = allCategories.Where(c => c.ParentCategoryId == null).ToList();

            return rootCategories.Select(c => BuildHierarchy(c, allCategories, 0)).ToList();
        }

        // Get only root categories (categories with no parent)
        public async Task<IEnumerable<CategoryDto>> GetRootCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = null,
                    ProductCount = c.Products.Count,
                    HasSubCategories = c.SubCategories.Any()
                })
                .ToListAsync();
        }

        // Get subcategories of a specific parent category
        public async Task<IEnumerable<CategoryDto>> GetSubCategoriesAsync(int parentId)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Where(c => c.ParentCategoryId == parentId)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : null,
                    ProductCount = c.Products.Count,
                    HasSubCategories = c.SubCategories.Any()
                })
                .ToListAsync();
        }

        // Helper method to build hierarchy recursively
        private CategoryHierarchyDto BuildHierarchy(Category category, List<Category> allCategories, int level)
        {
            var subcategories = allCategories.Where(c => c.ParentCategoryId == category.Id).ToList();

            return new CategoryHierarchyDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                ProductCount = category.Products.Count,
                Level = level,
                SubCategories = subcategories.Select(sub => BuildHierarchy(sub, allCategories, level + 1)).ToList()
            };
        }

        // Helper method to check if a category is a descendant of another
        private async Task<bool> IsDescendantAsync(int ancestorId, int potentialDescendantId)
        {
            var category = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == ancestorId);

            if (category == null) return false;

            return await CheckDescendantRecursive(category.SubCategories, potentialDescendantId);
        }

        private async Task<bool> CheckDescendantRecursive(ICollection<Category> categories, int targetId)
        {
            foreach (var category in categories)
            {
                if (category.Id == targetId) return true;

                var fullCategory = await _context.Categories
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.Id == category.Id);

                if (fullCategory != null && await CheckDescendantRecursive(fullCategory.SubCategories, targetId))
                    return true;
            }
            return false;
        }
    }
}
