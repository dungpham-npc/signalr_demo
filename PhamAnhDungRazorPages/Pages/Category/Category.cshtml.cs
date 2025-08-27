using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.Models;
using BusinessLayer.Services;

namespace PhamAnhDungRazorPages.Pages.Category;

public class CategoryModel : PageModel
{
    private readonly ICategoryService _categoryService;
    private readonly INewsArticleService _newsArticleService;

    public CategoryModel(ICategoryService categoryService, INewsArticleService newsArticleService)
    {
        _categoryService = categoryService;
        _newsArticleService = newsArticleService;
    }

    [BindProperty]
    public IEnumerable<DAL.Models.Category> Categories { get; set; }

    public void OnGet()
    {
        Categories = _categoryService.GetCategories();
    }

    private bool CheckIsStaff()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        const string staffRole = "1";
        return userRole == staffRole;
    }

    [BindProperty]
    public DAL.Models.Category Category { get; set; }

    public JsonResult OnPostCreate()
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = "Invalid data", errors });
            }

            _categoryService.CreateCategory(Category);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    public IActionResult OnGetGetCategory(int? id)
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        Categories = _categoryService.GetCategories().ToList();

        if (id is > 0)
        {
            Console.WriteLine($"Fetching category ID: {id.Value}");
            var category = _categoryService.GetCategoryById(id.Value);

            if (category == null)
            {
                Console.WriteLine($"Category {id.Value} not found!");
                return NotFound($"Category {id.Value} not found");
            }

            Console.WriteLine($"Category found: {category.CategoryName}");

            Category = new DAL.Models.Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription
            };

            return new JsonResult(Category);
        }

        // If no ID is provided, return an empty category
        Console.WriteLine("⚠ No ID provided, returning an empty category.");
        Category = new DAL.Models.Category();
        return new JsonResult(Category);
    }



    public JsonResult OnPostUpdate()
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new JsonResult(new { success = false, message = "Invalid data", errors });
            }


            _categoryService.UpdateCategory(Category.CategoryId, Category);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }

    public JsonResult OnPostDelete(int id)
    {
        if (!CheckIsStaff())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        try
        {
            var hasArticles = _newsArticleService.GetNewsArticles()
                .Any(a => a.CategoryId == id);

            if (hasArticles)
                return new JsonResult(new { success = false, message = "Cannot delete category that is used in articles" });

            _categoryService.DeleteCategory(id);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}