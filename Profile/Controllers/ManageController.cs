using Finbuckle.MultiTenant.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Profile.Attributes;
using Profile.Models;
using Profile.Models.Manage;
using Profile.Services;
using Profile.Tenant;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Profile.Controllers
{
    [Authorize]
    [SecurityHeaders]
    public class ManageController : Controller
    {
        private readonly TenantContext tenantContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ISmsSender smsSender;

        public ManageController(
            TenantContext tenantContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            this.tenantContext = tenantContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.smsSender = smsSender;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                await signInManager.SignOutAsync();
                return RedirectToAction(nameof(Index));
            }
            UserDashboardViewmodel vm = await GetUserDashboardViewmodel(user);
            return View(vm);
        }

        private async Task<UserDashboardViewmodel> GetUserDashboardViewmodel(ApplicationUser user)
        {
            var reqFields = tenantContext.Items[Tenant.Constants.REQUIREDFIELDS] as Field[];
            var optFields = tenantContext.Items[Tenant.Constants.OPTIONALFIELDS] as Field[];
            var socialLoginsConfigured = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
            var configuredLogins = SocialLoginProvider.Instance.Get.Where(sl => socialLoginsConfigured.ContainsKey(sl.Key));

            return new UserDashboardViewmodel
            {
                Email = user.Email,
                Username = System.Guid.TryParse(user.UserName, out System.Guid _) ? string.Empty : user.UserName,

                FirstName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName))?.Value ?? string.Empty,
                LastName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname))?.Value ?? string.Empty,

                SocialLogins = configuredLogins.ToDictionary(x => x, x => user.Logins.FirstOrDefault(l => l.LoginProvider.Equals(x.Key))?.ProviderKey),
                HasPassword = await userManager.HasPasswordAsync(user),

                Required = reqFields.Select(x => new ValueField(x) { Value = user.Claims.FirstOrDefault(c => c.Type.Equals(x.ClaimType))?.Value }).ToList(),
                Optional = optFields.Select(x => new ValueField(x) { Value = user.Claims.FirstOrDefault(c => c.Type.Equals(x.ClaimType))?.Value }).ToList()
            };
        }

        [HttpPost]
        public async Task<IActionResult> LinkExternalLogin(ExternalLoginModel externalLoginModel)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveExternalLogin(ExternalLoginModel externalLoginModel)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile(SaveProfileModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                if (model.Required.All(x => !string.IsNullOrEmpty(x.Value)))
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

                    var oldClaims = (await userManager.GetClaimsAsync(user)).GroupBy(x => x.Type).ToDictionary(x => x.Key);

                    var addClaims = new List<Claim>();
                    var removeClaims = new List<Claim>();

                    //instead of replace claim
                    //make a list of all to remove, and a list of all to add
                    foreach (var claim in claims)
                    {
                        if (oldClaims.TryGetValue(claim.Type, out IGrouping<string, Claim> old))
                        {
                            if (old.First().Value.Equals(claim.Value))
                            {
                                oldClaims.Remove(claim.Type);
                            } else
                            {
                                removeClaims.Add(old.First());
                                addClaims.Add(claim);
                            }
                        } else
                        {
                            addClaims.Add(claim);
                        }
                    }
                    foreach (var claimToRemove in oldClaims.Values.SelectMany(c => c))
                    {
                        removeClaims.Add(claimToRemove);
                    }
                    await userManager.RemoveClaimsAsync(user, removeClaims);
                    await userManager.AddClaimsAsync(user, addClaims);
                } else
                {
                    var vm = await GetUserDashboardViewmodel(user);
                    vm.Required = model.Required;
                    vm.Optional = model.Optional;
                    return View("Index", vm);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(SaveLoginModel saveLoginModel)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            if (user != null)
            {
                var errors = new List<string>();
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(saveLoginModel.Username))
                    {
                        var idResult = await userManager.SetUserNameAsync(user, saveLoginModel.Username);
                        if (idResult.Errors.Count() > 0) errors.AddRange(idResult.Errors.Select(x => x.Description));
                    }
                    if (!string.IsNullOrEmpty(saveLoginModel.Password))
                    {
                        if (string.IsNullOrEmpty(user.PasswordHash))
                        {
                            var token = await userManager.GeneratePasswordResetTokenAsync(user);
                            var idResult = await userManager.ResetPasswordAsync(user, token, saveLoginModel.Password);
                            if (idResult.Errors.Count() > 0) errors.AddRange(idResult.Errors.Select(x => x.Description));
                        }
                        else
                        {
                            //try and set password
                            var idResult = await userManager.ChangePasswordAsync(user, saveLoginModel.CurrentPassword, saveLoginModel.Password);
                            if (idResult.Errors.Count() > 0) errors.AddRange(idResult.Errors.Select(x => x.Description));
                        }
                    }
                }
                else
                {
                    errors.Add("invalid model");
                }

                if (errors.Count> 0)
                {
                    var vm = await GetUserDashboardViewmodel(user);
                    vm.Username = saveLoginModel.Username;
                    vm.Email = saveLoginModel.Email;
                    return View("Index", vm);
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}