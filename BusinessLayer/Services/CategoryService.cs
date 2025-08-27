using DAL.Models;
using DAL.Repositories;

namespace BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id) ?? throw new KeyNotFoundException("Category not found");
        }

        public Category CreateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required");

            // Set default values for new categories if not provided
            category.IsActive = true; // Set default to true for new categories
            category.CategoryDescription ??= string.Empty; // Set empty string if null

            return _categoryRepository.Add(category);
        }

        public Category UpdateCategory(int id, Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required");

            var existingCategory = GetCategoryById(id);

            // Update all properties with provided values
            existingCategory.CategoryName = category.CategoryName;

            if (category.ParentCategoryId != null)
            {
                existingCategory.ParentCategoryId = category.ParentCategoryId;
            }

            // Keep existing description if new one is null
            if (category.CategoryDescription != null)
            {
                existingCategory.CategoryDescription = category.CategoryDescription;
            }

            existingCategory.IsActive = true;

            return _categoryRepository.Update(existingCategory);
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            _categoryRepository.Delete(category);
        }
    }
}