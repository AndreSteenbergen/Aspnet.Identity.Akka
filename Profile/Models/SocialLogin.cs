using Finbuckle.MultiTenant.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using System;

namespace Profile.Models
{
    public struct SocialLogin
    {
        public SocialLogin(
            string key,
            string displayName,
            string fontAwesomeIconClass,
            string hexColor,
            Func<AuthenticationBuilder, AuthenticationBuilder> addSocialLoginDefaultOptions,
            Func<MultiTenantBuilder, MultiTenantBuilder> addSocialLoginTenantOptions)
        {
            Key = key;
            DisplayName = displayName;
            FontAwesomeIconClass = fontAwesomeIconClass;
            HexColor = hexColor;
            AddSocialLoginDefaultOptions = addSocialLoginDefaultOptions;
            AddSocialLoginTenantOptions = addSocialLoginTenantOptions;
        }

        public string Key { get; }
        public string DisplayName { get; }
        public string FontAwesomeIconClass { get; }
        public string HexColor { get; }
        public Func<AuthenticationBuilder, AuthenticationBuilder> AddSocialLoginDefaultOptions { get; }
        public Func<MultiTenantBuilder, MultiTenantBuilder> AddSocialLoginTenantOptions { get; }

        public override bool Equals(object obj)
        {
            if (!(obj is SocialLogin))
            {
                return false;
            }

            var login = (SocialLogin)obj;
            return Key == login.Key;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key);
        }
    }
}
