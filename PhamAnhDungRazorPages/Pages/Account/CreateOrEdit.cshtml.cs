using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLayer.Services;
using DAL.Models;

namespace PhamAnhDungRazorPages.Pages.Account
{
    public class CreateOrEditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public CreateOrEditModel(IAccountService accountService)
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

        public IActionResult OnGet(int? id)
        {
            if (id is > 0)
            {
                Account = _accountService.GetAccountById(id.Value);
                if (Account == null) return NotFound();
            }
            else
            {
                Account = new SystemAccount();
            }
            return Page();
        }

        public JsonResult OnPostCreateOrUpdate()
        {
            if (!CheckIsAdmin())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Invalid data" });
            }

            if (Account.AccountId == 0)
            {
                _accountService.CreateAccount(Account);
            }
            else
            {
                _accountService.UpdateAccount(Account, Account.AccountId);
            }

            return new JsonResult(new { success = true });
        }
    }
}