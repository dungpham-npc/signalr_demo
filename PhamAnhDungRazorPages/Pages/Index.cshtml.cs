using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PhamAnhDungRazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly IAccountService _accountService;
    private readonly IConfiguration _configuration;

    public IndexModel(IAccountService accountService, IConfiguration configuration)
    {
        _accountService = accountService;
        _configuration = configuration;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // If already logged in, redirect to home page
        if (HttpContext.Session.GetInt32("AccountID").HasValue)
        {
            return RedirectToPage("/News/Index");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Email and password are required.";
            return Page();
        }

        var account = _accountService.Authenticate(Email, Password);
        if (account != null)
        {
            // Store user info in session
            HttpContext.Session.SetInt32("AccountID", account.AccountId);
            HttpContext.Session.SetString("UserRole", account.AccountRole);

            // Redirect to home page after successful login
            return RedirectToPage("/News/Index");
        }

        // Show error if login fails
        ErrorMessage = "Invalid email or password.";
        return Page();
    }
}