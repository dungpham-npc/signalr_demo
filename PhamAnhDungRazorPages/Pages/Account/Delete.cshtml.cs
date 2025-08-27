using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamAnhDungRazorPages.Pages.Account;

public class DeleteModel : PageModel
{
    private readonly IAccountService _accountService;

    public DeleteModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public SystemAccount Account { get; set; }

    private bool CheckIsAdmin()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        const string adminRole = "3";
        return userRole == adminRole;
    }

    public IActionResult OnGet(int id)
    {
        if (!CheckIsAdmin())
        {
            return RedirectToPage("/Index");
        }
        Account = _accountService.GetAccountById(id);
        return Account == null ? NotFound() : Page();
    }

    public JsonResult OnPostDelete(int id)
    {
        if (!CheckIsAdmin())
        {
            return new JsonResult(new { success = false, message = "Access denied" });
        }
        try
        {
            var existingAccount = _accountService.GetAccountById(id);
            if (existingAccount == null)
            {
                return new JsonResult(new { success = false, message = "Account not found" }) { StatusCode = 404 };
            }

            _accountService.DeleteAccount(id);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
        }
    }
}
