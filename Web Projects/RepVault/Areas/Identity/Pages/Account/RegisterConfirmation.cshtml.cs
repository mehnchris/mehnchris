using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RepVault.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        public string? Email { get; set; }
        public bool DisplayConfirmAccountLink { get; set; }
        public string? EmailConfirmationUrl { get; set; }

        public IActionResult OnGet(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            Email = email;

            // Set this to false once real email sender is configured
            DisplayConfirmAccountLink = false;

            // Uncomment if testing locally without email
            //var userId = "your-user-id";
            //var code = "your-token";
            //EmailConfirmationUrl = Url.Page(
            //    "/Account/ConfirmEmail",
            //    pageHandler: null,
            //    values: new { userId = userId, code = code },
            //    protocol: Request.Scheme);

            return Page();
        }
    }
}
