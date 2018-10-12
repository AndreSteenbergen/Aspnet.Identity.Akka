using Finbuckle.MultiTenant.Core;
using Finbuckle.MultiTenant.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Profile.Tenant
{
    public class TenantStrategy : IMultiTenantStrategy
    {
        public string GetIdentifier(object context)
        {
            if (!(context is HttpContext))
                throw new MultiTenantException(null,
                    new ArgumentException("\"context\" type must be of type HttpContext", nameof(context)));

            var host = (context as HttpContext).Request.Host;
            return host.Host;
        }
    }

    public class TenantResolver : IMultiTenantStore
    {
        private readonly TenantConfiguration[] tenantConfigurations;
        private readonly Dictionary<string, TenantContext> hostConfigurations = new Dictionary<string, TenantContext>();

        public TenantResolver(IEnumerable<TenantConfiguration> tenantConfigurations)
        {
            this.tenantConfigurations = tenantConfigurations.ToArray();
        }

        public Task<TenantContext> GetByIdentifierAsync(string identifier)
        {
            if (!hostConfigurations.TryGetValue(identifier, out TenantContext context))
            {
                foreach (var testConfig in tenantConfigurations)
                {
                    if (testConfig.Identifier == "*")
                    {
                        hostConfigurations[identifier] = context = GetContextFromConfig(testConfig);
                        break;
                    }
                    try
                    {
                        Regex re = new Regex(testConfig.Identifier, RegexOptions.IgnoreCase);
                        if (re.IsMatch(identifier))
                        {
                            hostConfigurations[identifier] = context = GetContextFromConfig(testConfig);
                            break;
                        }
                    }
                    catch { }
                }
            }
            return Task.FromResult(context);
        }

        private TenantContext GetContextFromConfig(TenantConfiguration testConfig)
        {
            var result = new TenantContext(
                testConfig.Id,
                testConfig.Name,
                testConfig.Name,
                null,
                GetType(),
                GetType()
            );

            result.Items[Constants.THEME] = testConfig.Theme;
            result.Items[Constants.SLOGAN] = testConfig.Slogan;
            result.Items[Constants.REQUIREDFIELDS] = testConfig.RequiredFields;
            result.Items[Constants.OPTIONALFIELDS] = testConfig.OptionalFields;
            result.Items[Constants.SOCIALLOGINS] = testConfig.SocialLogins;
            return result;
        }

        public Task<bool> TryAdd(TenantContext context)
        {
            return Task.FromResult(false);
        }

        public Task<bool> TryRemove(string identifier)
        {
            return Task.FromResult(false);
        }
    }
}
