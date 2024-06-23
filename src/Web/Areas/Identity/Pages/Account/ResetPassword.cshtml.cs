using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tm20trial.Infrastructure.Identity;

namespace tm20trial.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResetPasswordModel : PageModel
{
    [BindProperty]
    public required InputModel Input { get; set; }
    
    public class InputModel
    {
        [Required(ErrorMessage = "Email adress is required")]
        [EmailAddress(ErrorMessage = "Email in bad format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password Required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? Code { get; set; }
    }

    public List<string> PasswordErrors { get; set; } = new List<string>();
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly ILogger<ResetPasswordModel> _logger;

    public ResetPasswordModel (UserManager<ApplicationUser> userManager, IMediator mediator, ILogger<ResetPasswordModel> logger)
    {
        _userManager = userManager;
        _mediator = mediator;
        _logger = logger;
    }
    
    public IActionResult OnGet(string? code = null)
    {
        if (code == null)
        {
            return BadRequest("A code must be provided.");
        }
        else
        {
            Input = new InputModel
            {
                Code = code
            };
            return Page();
        }
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || String.IsNullOrEmpty(Input.Email) 
                                || String.IsNullOrEmpty(Input.Password) 
                                || String.IsNullOrEmpty(Input.Code))
        {
            return Page();
        }

        var user = await _userManager.FindByEmailAsync(Input.Email);
        
        if (user == null)
        {
            _logger.LogInformation($"User {Input.Email} try to reset password, but user not found");
            // Don't reveal that the user does not exist
            return RedirectToPage("./ResetPasswordConfirmation");
        }

        var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation($"User {Input.Email} has reset their password");

            var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            if (!emailConfirmed)
            {
                _logger.LogInformation($"User {Input.Email} is not active, now it's");
                await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
            }

            return RedirectToPage("./ResetPasswordConfirmation");
        }

        PasswordErrors = result.Errors.Select(x => x.Code).ToList();

        return Page();
    }
}

