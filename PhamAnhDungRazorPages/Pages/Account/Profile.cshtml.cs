using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamAnhDungRazorPages.Pages.Account;

public class ProfileModel : PageModel
{
    private readonly IAccountService _accountService;

    public ProfileModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    private bool CheckIsStaff()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        const string staffRole = "1";
        return userRole == staffRole;
    }


    [BindProperty]
    public DAL.Models.SystemAccount Account { get; set; }

    public IActionResult OnGet()
    {
        var accountId = HttpContext.Session.GetInt32("AccountID");
        if (!accountId.HasValue)
        {
            return RedirectToPage("/Index");
        }

        Account = _accountService.GetAccountById(accountId.Value);
        if (Account == null)
        {
            return NotFound();
        }

        return Page();
    }

    public JsonResult OnPostUpdate()
    {
        try
        {
            var currentUserId = HttpContext.Session.GetInt32("AccountID");
            if (!currentUserId.HasValue)
            {
                return new JsonResult(new { success = false, message = "Session expired", redirectUrl = "/Index" });
            }
            var existingAccount = _accountService.GetAccountById(Account.AccountId);
            if (existingAccount == null)
            {
                return new JsonResult(new { success = false, message = "Account not found" });
            }

            if (!CheckIsStaff())
            {
                Account.AccountRole = existingAccount.AccountRole;
            }

            ModelState.Clear();
            TryValidateModel(Account);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return new JsonResult(new { success = false, message = "Invalid data", errors = string.Join(", ", errors) });
            }

            _accountService.UpdateAccount(Account, Account.AccountId);
            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { success = false, message = ex.Message });
        }
    }
}