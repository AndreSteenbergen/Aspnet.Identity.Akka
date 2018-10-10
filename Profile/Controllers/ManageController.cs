using Finbuckle.MultiTenant.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Attributes;

namespace Profile.Controllers
{
    [Authorize]
    [SecurityHeaders]
    public class ManageController : Controller
    {
        private readonly TenantContext tenantContext;

        public ManageController(TenantContext tenantContext)
        {
            this.tenantContext = tenantContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}