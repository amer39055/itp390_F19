using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterAdmin : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterAdmin> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IServiceProvider _serviceProvider;

        public RegisterAdmin(
            Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterAdmin> logger,
            IEmailSender emailSender,
            IServiceProvider serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _serviceProvider = serviceProvider;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "National Id")]
            public string NationalId { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "Address")]
            public string Address { get; set; }

            [Display(Name = "Home Location")]
            public string HomeLocation { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
            [DataType(DataType.Date)]
            [Display(Name = "Birth Date")]
            public string BirthDate { get; set; }

            [Display(Name = "Gender")]
            public string Gender { get; set; }

            [Display(Name = "User Type")]
            public string UserType { get; set; }

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
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    NationalId = Input.NationalId,
                    City = Input.City,
                    Address = Input.Address,
                    HomeLocation = Input.HomeLocation,
                    BirthDate = Input.BirthDate,
                    Gender = Input.Gender,
                    PhoneNumber = Input.PhoneNumber,
                    UserType = Input.UserType,
                    RegisteredDate = DateTime.Now,
                    Status = "Active",
                    FullName = Input.FirstName + " " + Input.LastName
                };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    //Add Claims To Users
                    var UserManager = _serviceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
                     
                    if (user.UserType == "Admin")
                    {
                        await UserManager.AddClaimAsync(user, new Claim("Admin", "true"));
                        await UserManager.AddClaimAsync(user, new Claim("CustomerService", "true"));
                        await UserManager.AddClaimAsync(user, new Claim(" Admin_CustomerService", "true"));

                    }
                    else if (user.UserType == "Customer Service")
                    {
                        await UserManager.AddClaimAsync(user, new Claim("CustomerService", "true"));
                        await UserManager.AddClaimAsync(user, new Claim("Admin_CustomerService", "true"));

                        
                    }

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
