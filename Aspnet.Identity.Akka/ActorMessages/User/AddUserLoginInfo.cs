using Aspnet.Identity.Akka.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class AddUserLoginInfo : IUserPropertyChange
    {
        public AddUserLoginInfo(UserLoginInfo loginInfo) : this(loginInfo.LoginProvider, loginInfo.ProviderDisplayName, loginInfo.ProviderKey) {  }

        public AddUserLoginInfo(string loginProvider, string providerDisplayName, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderDisplayName = providerDisplayName;
            ProviderKey = providerKey;
        }

        public string LoginProvider { get; }
        public string ProviderDisplayName { get; }
        public string ProviderKey { get; }        
    }
}