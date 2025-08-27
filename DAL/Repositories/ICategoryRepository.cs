using System.Collections.Generic;
using DAL.Models;

namespace DAL.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(int id);
        Category Add(Category category);
        Category Update(Category category);
        void Delete(Category category);
    }
}