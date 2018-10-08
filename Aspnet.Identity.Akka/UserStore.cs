using Akka.Actor;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Aspnet.Identity.Akka
{
    public class
            UserStore<TKey, TUser> :
            IUserLoginStore<TUser>,
            IUserClaimStore<TUser>,
            IUserPasswordStore<TUser>,
            IUserSecurityStampStore<TUser>,
            IUserAuthenticationTokenStore<TUser>,
            IUserEmailStore<TUser>,
            IUserLockoutStore<TUser>,
            IUserTwoFactorStore<TUser>,
            IUserPhoneNumberStore<TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private readonly IActorRef userCoordinator;

        public UserStore(IActorRef userCoordinator)
        {
            this.userCoordinator = userCoordinator;
        }

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureClaimsLoaded(user);
            foreach (var item in claims)
            {
                user.Claims.Add(item);
            }
            user.Changes.Add(new AddClaims(claims));
        }

        private async Task EnsureClaimsLoaded(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (user.Claims == null)
            {
                user.Claims = (await userCoordinator.Ask<IEnumerable<Claim>>(new RequestClaims<TKey>(user.Id))).ToList();
            }
        }

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureLoginsLoaded(user);
            user.Logins.Add(new ImmutableUserLoginInfo(login.LoginProvider, login.ProviderKey, login.ProviderDisplayName));
        }

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return userCoordinator.Ask<IdentityResult>(new CreateUser<TKey, TUser>(user), cancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return userCoordinator.Ask<IdentityResult>(new DeleteUser<TKey>(user.Id), cancellationToken);
        }

        public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var rst = await userCoordinator.Ask<object>(new FindByEmail(normalizedEmail));
            return rst as TUser;
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var rst = await userCoordinator.Ask<object>(new FindById(userId));
            return rst as TUser;
        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var rst = await userCoordinator.Ask<object>(new FindByLogin(new ExternalLogin(loginProvider, providerKey)));
            return rst as TUser;
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var rst = await userCoordinator.Ask<object>(new FindByUsername(normalizedUserName));
            return rst as TUser;
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureClaimsLoaded(user);
            return user.Claims;
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LockoutEnd);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureLoginsLoaded(user);
            return user.Logins.Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.DisplayName)).ToList();
        }

        private async Task EnsureLoginsLoaded(TUser user)
        {
            if (user.Logins == null)
            {
                user.Logins = (await userCoordinator.Ask<IEnumerable<ImmutableUserLoginInfo>>(new RequestUserLoginInfo<TKey>(user.Id))).ToList();
            }
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.Id.ToString());
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetAccessFailesCount(user.AccessFailedCount++));
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureClaimsLoaded(user);

            var claimsToFind = claims.GroupBy(c => c.Type).ToDictionary(x => x.Key, x => new HashSet<string>(x.GroupBy(c => c.Value).Select(c => c.Key)));
            var claimsToDelete = user.Claims.Where(c => claimsToFind.TryGetValue(c.Type, out HashSet<string> hs) && hs.Contains(c.Value)).ToList();

            var resultSet = new List<Claim>();
            var removeClaims = new List<Claim>();
            foreach (var c in user.Claims)
            {
                if (claimsToFind.TryGetValue(c.Type, out HashSet<string> hs) && hs.Contains(c.Value))
                {
                    removeClaims.Add(c);
                }
                else
                {
                    resultSet.Add(c);
                }
            }
            user.Claims = resultSet;
            if (removeClaims.Count > 0)
            {
                user.Changes.Add(new RemoveClaims(removeClaims));
            }
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await EnsureLoginsLoaded(user);
            var login = user.Logins.FirstOrDefault(x => x.LoginProvider.Equals(loginProvider) && x.ProviderKey.Equals(providerKey));
            if (login != default(ImmutableUserLoginInfo))
            {
                user.Logins.Remove(login);
                user.Changes.Add(new RemoveLogin(loginProvider, providerKey));
            }
        }

        public async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            if (token != default(ImmutableIdentityUserToken<TKey>))
            {
                user.Tokens.Remove(token);
                user.Changes.Add(new RemoveToken(loginProvider, name));
            }
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await RemoveClaimsAsync(user, new[] { claim }, cancellationToken);
            await AddClaimsAsync(user, new[] { newClaim }, cancellationToken);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetAccessFailesCount(user.AccessFailedCount = 0));
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetEmail(user.Email = email));
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetEmailConfirmed(user.EmailConfirmed = confirmed));
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetLockoutEnabled(user.LockoutEnabled = enabled));
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetLockoutEndDate(user.LockoutEnd = lockoutEnd));
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetEmail(user.NormalizedEmail = normalizedEmail));
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetEmail(user.NormalizedUserName = normalizedName));
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetPasswordHash(user.PasswordHash = passwordHash));
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetPhoneNumber(user.PhoneNumber = phoneNumber));
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetPhoneNumberConfirmed(user.PhoneNumberConfirmed = confirmed));
            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetSecurityStamp(user.SecurityStamp = stamp));
            return Task.CompletedTask;
        }

        private async Task EnsureTokensLoaded(TUser user, CancellationToken cancellationToken)
        {
            if (user.Tokens == null)
            {
                var tokens = (await userCoordinator.Ask<IEnumerable<ImmutableIdentityUserToken<TKey>>>(new RequestTokens<TKey>(user.Id), cancellationToken));
                user.Tokens = tokens as List<ImmutableIdentityUserToken<TKey>> ?? tokens.ToList();
            }
        }

        private async Task<ImmutableIdentityUserToken<TKey>> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            //just to be sure
            await EnsureTokensLoaded(user, cancellationToken);
            return user.Tokens.FirstOrDefault(t => t.LoginProvider.Equals(loginProvider) && t.Name == name);
        }

        public async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrEmpty(loginProvider))
            {
                throw new ArgumentNullException(nameof(loginProvider));
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            await EnsureTokensLoaded(user, cancellationToken);
            var token = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            if (token != default(ImmutableIdentityUserToken<TKey>))
            {
                user.Tokens.Remove(token);
            }
            user.Tokens.Add(new ImmutableIdentityUserToken<TKey>(user.Id, loginProvider, name, value));
            user.Changes.Add(new SetToken(loginProvider, name, value));
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetTwoFactorEnabled(user.TwoFactorEnabled = enabled));
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Changes.Add(new SetUserName(user.UserName = userName));
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return userCoordinator.Ask<IdentityResult>(new UpdateUser<TKey, TUser>(user));
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.UserName);
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var rst = await userCoordinator.Ask<IEnumerable<TUser>>(new FindByClaim(claim.Type, claim.Value), cancellationToken);
            return rst as List<TUser> ?? rst.ToList();
        }

        public async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await EnsureTokensLoaded(user, cancellationToken);
            var token = user.Tokens.FirstOrDefault(x => x.LoginProvider.Equals(loginProvider) && x.Name.Equals(name));
            return token?.Value;
        }

        public void Dispose()
        {
            //do nothing?
        }
    }
}
