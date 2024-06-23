using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using tm20trial.Application.Common.Enums;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Infrastructure.Identity;

namespace tm20trial.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    [BindProperty]
    public required InputModel Input { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email adress is required")]
        [EmailAddress(ErrorMessage = "Email in bad format.")]
        public required string Email { get; set; }
    }
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;
    private readonly IEmailClient _emailClient;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IMediator mediator, IEmailClient emailClient, IIdentityService identityService, IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _userManager = userManager;
        _mediator = mediator;
        _emailClient = emailClient;
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<IActionResult> OnPostAsync()
    {

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
            }

            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            await _emailClient.SendEmailTemplateAsync(
                new Email
                {
                    To = new[] { new EmailPerson { Email = Input.Email } },
                    Subject = "Trials classics: reset password",
                },
                await _emailClient.GetTemplateIdAsync(TypeTemplateEmail.ResetPassword),
                new JObject
                {
                    {"link", callbackUrl}
                });

            return RedirectToPage("/Account/ForgotPasswordConfirmation");
        }
        return Page();
    }
    
    public void OnGet()
    {
        
    }
    
    
    public IActionResult GoBack()
    {
        return Redirect(Request.Headers["Login"].ToString());
    }
}
