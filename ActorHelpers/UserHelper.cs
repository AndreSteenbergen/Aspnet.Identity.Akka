using Akka.Actor;
using Aspnet.Identity.Akka.ActorMessages;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.ActorHelpers
{
    public class UserHelper<TKey, TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private bool inSync;
        private List<Tuple<IActorRef, ICommand, Action<IEvent, Action<IEvent>>>> stash = new List<Tuple<IActorRef, ICommand, Action<IEvent, Action<IEvent>>>>();
        private readonly TUser identityUser;

        public UserHelper(TUser identityUser)
        {
            this.identityUser = identityUser;
        }

        public virtual void SetInSync(bool inSync)
        {
            this.inSync = inSync;
            foreach (var cmd in stash)
            {
                OnCommand(cmd.Item1, cmd.Item2, cmd.Item3);
            }
            stash.Clear();
        }

        public virtual void OnCommand(IActorRef sender, ICommand message, Action<IEvent, Action<IEvent>> persist)
        {
            switch (message)
            {
                case UpdateUser<TKey, TUser> evt:
                    HandleCommand(sender, evt, persist);
                    break;
                case CreateUser<TKey, TUser> evt:
                    HandleCommand(sender, evt, persist);
                    break;
                case DeleteUser<TKey> evt:
                    HandleCommand(sender, evt, persist);
                    break;
            }
        }

        private void HandleCommand(IActorRef sender, DeleteUser<TKey> evt, Action<IEvent, Action<IEvent>> persist)
        {
            throw new NotImplementedException();
        }

        private void HandleCommand(IActorRef sender, CreateUser<TKey, TUser> evt, Action<IEvent, Action<IEvent>> persist)
        {
            throw new NotImplementedException();
        }

        private void HandleCommand(IActorRef sender, UpdateUser<TKey, TUser> evt, Action<IEvent, Action<IEvent>> persist)
        {
            var events = new List<IEvent>();
            foreach (var change in evt.User.Changes)
            {
                if (TestCommand(change, out IEvent e))
                {
                    if (e != null)
                    {
                        events.Add(e);
                    }
                }
                else
                {
                    sender.Tell(IdentityResult.Failed());
                    return;
                }
            }
            foreach (var @event in events)
            {
                persist(@event, e => OnEvent(sender, e));
            }
        }

        private bool TestCommand(IUserPropertyChange change, out IEvent e)
        {
            switch (change)
            {
                case AddClaims evt:
                    return TestCommand(evt, out e);
                case AddUserLoginInfo evt:
                    return TestCommand(evt, out e);
                case RemoveClaims evt:
                    return TestCommand(evt, out e);
                case RemoveLogin evt:
                    return TestCommand(evt, out e);
                case RemoveToken evt:
                    return TestCommand(evt, out e);
                case SetEmail evt:
                    return TestCommand(evt, out e);
                case SetEmailConfirmed evt:
                    return TestCommand(evt, out e);
                case SetLockoutEnabled evt:
                    return TestCommand(evt, out e);
                case SetLockoutEndDate evt:
                    return TestCommand(evt, out e);
                case SetNormalizedEmail evt:
                    return TestCommand(evt, out e);
                case SetPasswordHash evt:
                    return TestCommand(evt, out e);
                case SetPhoneNumber evt:
                    return TestCommand(evt, out e);
                case SetPhoneNumberConfirmed evt:
                    return TestCommand(evt, out e);
                case SetSecurityStamp evt:
                    return TestCommand(evt, out e);
                case SetToken evt:
                    return TestCommand(evt, out e);
                case SetTwoFactorEnabled evt:
                    return TestCommand(evt, out e);
                case SetUserName evt:
                    return TestCommand(evt, out e);
            }
            e = null;
            return false;
        }

        private TUser user = null;

        private bool TestCommand(SetUserName evt, out IEvent e)
        {
            e = null;
            if (user != null && !string.Equals(user.UserName, evt.UserName))
            {
                e = new UserNameChanged(evt.UserName);
            }

            //add some tests?
            return true;
        }

        private bool TestCommand(SetTwoFactorEnabled evt, out IEvent e)
        {
            e = null;
            if (user != null && user.TwoFactorEnabled != evt.TwoFactorEnabled)
            {
                e = new TwoFactorEnabledChanged(evt.TwoFactorEnabled);
            }

            //add some tests?
            return true;
        }

        private bool TestCommand(SetToken evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.Tokens == null)
            {
                e = new TokenAdded(evt.LoginProvider, evt.Name, evt.Value);
            }
            else
            {
                ImmutableIdentityUserToken<TKey> token = user.Tokens.FirstOrDefault(x => x.LoginProvider.Equals(evt.LoginProvider) && x.Name.Equals(evt.Name));
                if (token == default(ImmutableIdentityUserToken<TKey>))
                {
                    e = new TokenAdded(evt.LoginProvider, evt.Name, evt.Value);
                }
                else if (token.Value.Equals(evt.Value))
                {
                    e = new TokenUpdated(evt.LoginProvider, evt.Name, evt.Value);
                }
            }

            return true;
        }

        private bool TestCommand(SetSecurityStamp evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!string.Equals(user.SecurityStamp, evt.Stamp))
            {
                e = new SecurityStampChanged(evt.Stamp);
            }
            return true;
        }

        private bool TestCommand(SetPhoneNumberConfirmed evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.PhoneNumberConfirmed != evt.PhoneNumberConfirmed)
            {
                e = new PhoneNumberConfirmed(evt.PhoneNumberConfirmed);
            }
            return true;
        }

        private bool TestCommand(SetPhoneNumber evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!string.Equals(user.PhoneNumber, evt.PhoneNumber))
            {
                e = new PhoneNumberConfirmed(evt.PhoneNumberConfirmed);
            }
            return true;
        }

        private bool TestCommand(SetPasswordHash evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!string.Equals(user.PasswordHash, evt.PasswordHash))
            {
                e = new PasswordHashChanged(evt.PasswordHash);
            }
            return true;
        }

        private bool TestCommand(SetNormalizedEmail evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!string.Equals(user.NormalizedEmail, evt.Email))
            {
                e = new NormalizedEmailChanged(evt.Email);
            }
            return true;
        }

        private bool TestCommand(SetLockoutEndDate evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if ((evt.LockoutEnd == null && user.LockoutEnd != null) || !evt.LockoutEnd.Equals(user.LockoutEnd))
            {
                e = new LockoutEndDateChanged(evt.LockoutEnd);
            }
            return true;
        }

        private bool TestCommand(SetLockoutEnabled evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!user.LockoutEnabled != evt.LockoutEnabled)
            {
                e = new LockoutEnabledChanged(evt.LockoutEnabled);
            }
            return true;
        }

        private bool TestCommand(SetEmailConfirmed evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.EmailConfirmed != evt.Confirmed)
            {
                e = new EmailConfirmed(evt.Confirmed);
            }
            return true;
        }

        private bool TestCommand(SetEmail evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (!string.Equals(user.Email, evt.Email))
            {
                e = new EmailChanged(evt.Email);
            }
            return true;
        }

        private bool TestCommand(RemoveToken evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.Tokens == null)
            {
                return true;
            }

            var token = user.Tokens.FirstOrDefault(x => x.LoginProvider.Equals(evt.LoginProvider) && x.Name.Equals(evt.Name));
            if (token != default(ImmutableIdentityUserToken<TKey>))
            {
                e = new TokenRemoved(evt.LoginProvider, evt.Name);
            }

            return true;
        }

        private bool TestCommand(RemoveLogin evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.Logins == null)
            {
                return true;
            }

            var login = user.Logins.FirstOrDefault(x => x.LoginProvider.Equals(evt.LoginProvider) && x.ProviderKey.Equals(evt.ProviderKey));
            if (login == default(ImmutableUserLoginInfo))
            {
                e = new LoginRemoved(evt.LoginProvider, evt.ProviderKey);
            }

            return true;
        }

        private bool TestCommand(RemoveClaims evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.Claims == null)
            {
                return true;
            }

            var claimsToRemove = user.Claims.Union(evt.Claims, ClaimComparer.Instance).ToList();
            if (claimsToRemove.Count > 0)
            {
                e = new ClaimsRemoved(claimsToRemove);
            }

            return true;
        }

        private bool TestCommand(AddUserLoginInfo evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            if (user.Logins == null)
            {
                e = new UserLoginInfoAdded(evt.LoginProvider, evt.ProviderDisplayName, evt.ProviderKey);
            } else
            {
                var login = user.Logins.FirstOrDefault(x => x.LoginProvider.Equals(evt.LoginProvider) && x.ProviderKey.Equals(evt.ProviderKey));
                if (login == default(ImmutableUserLoginInfo))
                {
                    e = new UserLoginInfoAdded(evt.LoginProvider, evt.ProviderDisplayName, evt.ProviderKey);
                }
            }

            return true;
        }

        private bool TestCommand(AddClaims evt, out IEvent e)
        {
            e = null;
            if (user == null)
            {
                return false;
            }

            var claimsToAdd = evt.Claims.Except(user.Claims, ClaimComparer.Instance).ToList();
            if (claimsToAdd.Count > 0)
            {
                e = new ClaimsAdded(claimsToAdd);
            }
            return true;
        }

        public virtual void OnEvent(IActorRef sender, IEvent message)
        {

        }
    }
}
