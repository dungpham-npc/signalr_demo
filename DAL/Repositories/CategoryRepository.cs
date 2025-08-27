using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsmanagementContext _context;

        public CategoryRepository(FunewsmanagementContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.Where(c => c.IsActive == true).ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            var existing = _context.Categories.Find(category.CategoryId);
            if (existing == null) return null!;

            _context.Entry(existing).CurrentValues.SetValues(category);
            _context.SaveChanges();
            return category;
        }

        public void Delete(Category category)
        {
            var categoryToDelete = _context.Categories.Find(category.CategoryId);
            if (categoryToDelete == null) return;
            categoryToDelete.IsActive = false;
            _context.SaveChanges();
        }
    }
}