using DAL.Models;

namespace BusinessLayer.Services;

public interface ICategoryService
{
    IEnumerable<Category> GetCategories();
    Category GetCategoryById(int id);
    Category CreateCategory(Category category);
    Category UpdateCategory(int id, Category category);
    void DeleteCategory(int id);
}