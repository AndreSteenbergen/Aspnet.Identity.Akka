using Finbuckle.MultiTenant.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profile.Attributes;
using Profile.Models;
using Profile.Models.Account;
using Profile.Services;
using Profile.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Profile.Controllers
{
    // For more information on how to enable account confirmation and
    // password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
    [SecurityHeaders]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly TenantContext tenantContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;

        public AccountController(
            TenantContext tenantContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            this.tenantContext = tenantContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewmodel(returnUrl));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewmodel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(model.ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    // return RedirectToAction(nameof(SendCode), new { model.ReturnUrl });
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(new LoginViewmodel(model));
                }
            }
            return View(new LoginViewmodel(model));
        }

        [HttpGet]
        public IActionResult Registreer(string returnUrl = null)
        {
            var reqFields = tenantContext.Items[Tenant.Constants.REQUIREDFIELDS] as Field[];
            var optFields = tenantContext.Items[Tenant.Constants.OPTIONALFIELDS] as Field[];

            var vm = new RegisterViewmodel
            {
                ReturnUrl = returnUrl,
                Required = reqFields.Select(x => new ValueField(x)).ToList(),
                Optional = optFields.Select(x => new ValueField(x)).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registreer(RegisterViewmodel model, string returnUrl = null)
        {
            if (ModelState.IsValid && model.Required.All(x => !string.IsNullOrEmpty(x.Value)))
            {
                var user = new ApplicationUser(Guid.NewGuid())
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>();
                    foreach (var required in model.Required.Where(x => !string.IsNullOrEmpty(x.Value)))
                    {
                        claims.Add(new Claim(required.ClaimType, required.Value));
                    }
                    foreach (var optional in model.Optional.Where(x => !string.IsNullOrEmpty(x.Value)))
                    {
                        claims.Add(new Claim(optional.ClaimType, optional.Value));
                    }
                    await userManager.AddClaimsAsync(user, claims);

                    // Send an email with this link
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    await emailSender.SendEmailAsync(model.Email, $"Welkom bij {tenantContext.Name}", $"Om gebruik te maken van uw {tenantContext.Name} account moet u uw email adres bevestigen. Gebruik daarvoor deze link: <a href='{callbackUrl}'>Activeer nu</a>");

                    return View("Registered");
                }
                AddErrors(result);
            }
            return View(model);
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = new LogoutViewmodel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = true
            };

            if (!vm.ShowLogoutPrompt)
            {
                // no need to show prompt
                return await Logout(vm);
            }
            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewmodel model)
        {
            var vm = new LoggedOutViewModel
            {
                LogoutId = model.LogoutId
            };

            await signInManager.SignOutAsync();
            return View("LoggedOut", vm);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public IActionResult WachtwoordVergeten()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WachtwoordVergeten(ForgotPasswordViewmodel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action(nameof(AccountController.ResetPassword), "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                await emailSender.SendEmailAsync(model.Email, "Wachtwoord resetten", $"U kunt uw wachtwoord resetten door op de volgende link te klikken: <a href='{callbackUrl}'>link</a>.");

                return RedirectToAction(nameof(AccountController.WachtwoordVergetenBevestiging), "Account");
            }

            // If we got this far, something failed, redisplay form
            return View("ForgotPassword", model);
        }

        [HttpGet]
        public IActionResult WachtwoordVergetenBevestiging()
        {
            return View("ForgotPasswordConfirmation");
        }


        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewmodel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //External logins
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var loginProvider = info.LoginProvider;
            // Sign in the user with this external login provider if the user already has a login.
            var user = await userManager.FindByLoginAsync(loginProvider, info.ProviderKey);
            var result = await signInManager.ExternalLoginSignInAsync(loginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                var reqFields = tenantContext.Items[Tenant.Constants.REQUIREDFIELDS] as Field[];
                var optFields = tenantContext.Items[Tenant.Constants.OPTIONALFIELDS] as Field[];

                var vm = new ExternalLoginConfirmationViewmodel
                {
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                    ReturnUrl = returnUrl,
                    Required = reqFields.Select(x => new ValueField(x) { Value = info.Principal.FindFirstValue(x.ClaimType) }).ToList(),
                    Optional = optFields.Select(x => new ValueField(x) { Value = info.Principal.FindFirstValue(x.ClaimType) }).ToList()
                };

                // If the user does not have an account, then ask the user to create an account.                
                return View("ExternalLoginConfirmation", vm);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewmodel model, string returnUrl = null)
        {
            if (ModelState.IsValid && model.Required.All(x => !string.IsNullOrEmpty(x.Value)))
            {
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser(Guid.NewGuid())
                {
                    UserName = Guid.NewGuid().ToString(),
                    Email = model.Email
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddLoginAsync(user, info);

                    var claims = new List<Claim>();
                    claims.Add(info.Principal.FindFirst(ClaimTypes.NameIdentifier));

                    foreach (var required in model.Required.Where(x => !string.IsNullOrEmpty(x.Value)))
                    {
                        claims.Add(new Claim(required.ClaimType, required.Value));
                    }
                    foreach (var optional in model.Optional.Where(x => !string.IsNullOrEmpty(x.Value)))
                    {
                        claims.Add(new Claim(optional.ClaimType, optional.Value));
                    }

                    var phoneClaim = claims.Find(x => x.Type.Equals(ClaimTypes.MobilePhone));
                    if (phoneClaim != default(Claim))
                    {
                        await userManager.SetPhoneNumberAsync(user, phoneClaim.Value);
                    }
                    await userManager.AddClaimsAsync(user, claims);

                    // Send an email with this link
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    await emailSender.SendEmailAsync(model.Email, $"Welkom bij {tenantContext.Name}", $"Om gebruik te maken van uw {tenantContext.Name} account moet u uw email adres bevestigen. Gebruik daarvoor deze link: <a href='{callbackUrl}'>Activeer nu</a>");

                    return View("Registered");
                }
                AddErrors(result);
            }
            return View(model);
        }

        void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(ManageController.Index), "Manage");
            }
        }
    }
}