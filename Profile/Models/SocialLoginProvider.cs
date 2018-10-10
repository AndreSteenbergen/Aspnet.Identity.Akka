using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OAuth.LinkedIn;
using Finbuckle.MultiTenant.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.Extensions.DependencyInjection;

namespace Profile.Models
{
    public class SocialLoginProvider
    {
        public SocialLoginProvider()
        {
            Get = new[]
            {
                new SocialLogin("google", "Google", "fa-google", "#DD4B39", AddGoogleDefault, AddGoogleTenant),
                new SocialLogin("microsoft", "Microsoft", "fa-windows", "#00a1f1", AddMicrosoftDefault, AddMicrosoftTenant),
                new SocialLogin("facebook", "Facebook", "fa-facebook-f", "#3B5998", AddFacebookDefault, AddFacebookTenant),
                new SocialLogin("twitter", "Twitter", "fa-twitter", "#55ACEE", AddTwitterDefault, AddTwitterTenant),
                new SocialLogin("linkedin", "LinkedIn", "fa-linkedin-in", "#007bb5", AddLinkedInDefault, AddLinkedInTenant),
            };
        }

        //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/other-logins?view=aspnetcore-2.1
        private AuthenticationBuilder AddLinkedInDefault(AuthenticationBuilder arg)
        {
            return arg.AddLinkedIn("linkedin", liOptions =>
            {
                liOptions.ClientId = "default";
                liOptions.ClientSecret = "default";
            });
        }

        private AuthenticationBuilder AddTwitterDefault(AuthenticationBuilder arg)
        {
            return arg.AddTwitter("twitter", twOptions =>
            {
                twOptions.ConsumerKey = "default";
                twOptions.ConsumerSecret = "default";
            });
        }

        private AuthenticationBuilder AddFacebookDefault(AuthenticationBuilder arg)
        {
            return arg.AddFacebook("facebook", fbOptions =>
            {
                fbOptions.AppId = "default";
                fbOptions.AppSecret = "default";

                fbOptions.Scope.Add("email");
                fbOptions.Fields.Add("name");
                fbOptions.Fields.Add("email");

                fbOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        try
                        {
                            var id = context.Identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                            context.Identity.AddClaim(new Claim("ProfileImage", $"http://graph.facebook.com/{id}/picture?type=large&redirect=true&width=400&height=400", ClaimValueTypes.String, "Facebook", "Facebook"));
                        }
                        catch { }
                        return Task.FromResult(0);
                    }
                };
            });
        }

        private AuthenticationBuilder AddMicrosoftDefault(AuthenticationBuilder arg)
        {
            return arg.AddMicrosoftAccount("microsoft", msOptions =>
            {
                msOptions.ClientId = "default";
                msOptions.ClientSecret = "default";
            });
        }

        private AuthenticationBuilder AddGoogleDefault(AuthenticationBuilder arg)
        {
            return arg.AddGoogle("google", googleOptions =>
            {
                googleOptions.ClientId = "default";
                googleOptions.ClientSecret = "default";

                googleOptions.Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        try
                        {
                            var userLogo = context.User["image"].Value<string>("url");
                            context.Identity.AddClaim(new Claim("ProfileImage", userLogo.Replace("?sz=50", "?sz=400"), ClaimValueTypes.String, "Google", "Google"));
                        }
                        catch { }
                        return Task.FromResult(0);
                    }
                };
            });
        }

        private MultiTenantBuilder AddLinkedInTenant(MultiTenantBuilder arg)
        {
            return arg.WithPerTenantOptions<LinkedInAuthenticationOptions>((options, tenantContext) => {
                var socialLogins = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
                if (socialLogins == null || !socialLogins.TryGetValue("linkedin", out Dictionary<string, string> settings)) return;

                options.ClientId = settings["ClientId"];
                options.ClientSecret = settings["ClientSecret"];
            });
        }

        private MultiTenantBuilder AddTwitterTenant(MultiTenantBuilder arg)
        {
            return arg.WithPerTenantOptions<TwitterOptions>((options, tenantContext) => {
                var socialLogins = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
                if (socialLogins == null || !socialLogins.TryGetValue("twitter", out Dictionary<string, string> settings)) return;

                options.ConsumerKey = settings["ConsumerKey"];
                options.ConsumerSecret = settings["ConsumerSecret"];
            });
        }

        private MultiTenantBuilder AddFacebookTenant(MultiTenantBuilder arg)
        {
            return arg.WithPerTenantOptions<FacebookOptions>((options, tenantContext) => {
                var socialLogins = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
                if (socialLogins == null || !socialLogins.TryGetValue("facebook", out Dictionary<string,string> settings)) return;

                options.AppId = settings["AppId"];
                options.AppSecret = settings["AppSecret"];
            });
        }

        private MultiTenantBuilder AddMicrosoftTenant(MultiTenantBuilder arg)
        {
            return arg.WithPerTenantOptions<MicrosoftAccountOptions>((options, tenantContext) => {
                var socialLogins = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
                if (socialLogins == null || !socialLogins.TryGetValue("microsoft", out Dictionary<string, string> settings)) return;

                options.ClientId = settings["ClientId"];
                options.ClientSecret = settings["ClientSecret"];
            });
        }

        private MultiTenantBuilder AddGoogleTenant(MultiTenantBuilder arg)
        {
            return arg.WithPerTenantOptions<GoogleOptions>((options, tenantContext) => {
                var socialLogins = tenantContext.Items[Tenant.Constants.SOCIALLOGINS] as Dictionary<string, Dictionary<string, string>>;
                if (socialLogins == null || !socialLogins.TryGetValue("google", out Dictionary<string, string> settings)) return;

                options.ClientId = settings["ClientId"];
                options.ClientSecret = settings["ClientSecret"];
            });
        }


        public SocialLogin[] Get { get; }

        public static readonly SocialLoginProvider Instance = new SocialLoginProvider();
    }
}
