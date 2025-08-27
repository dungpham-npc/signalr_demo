using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLayer.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

namespace PhamAnhDungRazorPages.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
            Accounts = [];
        }

        public List<SystemAccount> Accounts { get; set; }
        private bool CheckIsAdmin()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            const string adminRole = "3";
            return userRole == adminRole;
        }

        public IActionResult OnGet()
        {
            if (!CheckIsAdmin())
            {
                return new JsonResult(new { success = false, message = "Access denied" });
            }
            Accounts = _accountService.GetAccounts().ToList();
            return Page();
        }
    }
}