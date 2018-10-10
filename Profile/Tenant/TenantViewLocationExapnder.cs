using Finbuckle.MultiTenant.AspNetCore;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Profile.Tenant
{
    public class TenantViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var ctx = context.ActionContext.HttpContext.GetTenantContext();
            context.Values[Constants.THEME] = (string) ctx.Items[Constants.THEME];
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(Constants.THEME, out string theme))
            {
                viewLocations = new[] {
                    $"/Themes/{theme}/{{1}}/{{0}}.cshtml",
                    $"/Themes/{theme}/Shared/{{0}}.cshtml",
                }
                .Concat(viewLocations);
            }
            return viewLocations;
        }
    }
}
