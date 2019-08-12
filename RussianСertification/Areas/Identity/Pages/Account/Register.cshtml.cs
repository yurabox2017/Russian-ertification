using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RussianСertification.Models;

namespace RussianСertification.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                try
                {

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        //var callbackUrl = Url.Action(
                        //   "ConfirmEmail",
                        //   "Account",
                        //   new { userId = user.Id, code = code },
                        //   protocol: HttpContext.Request.Scheme);
                        //EmailService emailService = new EmailService();
                        //await emailService.SendEmailAsync(Input.Email, "Confirm your account",
                        //    $": <a href='{callbackUrl}'>link</a>");

                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);
                        EmailService _emailService = new EmailService();
                        await _emailService.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>здесь</a>.");

                   //     await _signInManager.SignInAsync(user, isPersistent: false); //авто подтверждение почты

                        await _userManager.AddToRoleAsync(user, "гость");

                        return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                        // return LocalRedirect(returnUrl);
                    }
                }
                catch (Exception ex)
                {
                    await _userManager.DeleteAsync(user);
                    ModelState.AddModelError(string.Empty, ex.Message);
                    //foreach (var error in ex.Data)
                    //{


                    //}
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
