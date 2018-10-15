using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using Aspnet.Identity.Akka.Serialization.MsgPack;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace AspNet.Identity.Akka.Test.Serialization
{
    public class TestSerializationOfEvents
    {
        [Fact]
        public void TestSerializationOfUserCreated()
        {
            var userId = Guid.NewGuid();
            var evt = new UserCreated<Guid>(userId, "NORMALIZED USERNAME", "NORMALIZED EMAIL");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserCreated<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(userId, eventChk.UserId);
            Assert.Equal(evt.NormalizedUserName, eventChk.NormalizedUserName);
            Assert.Equal(evt.NormalizedEmail, eventChk.NormalizedEmail);
        }

        [Fact]
        public void TestSerializationOfClaimsAdded()
        {
            var evt = new ClaimsAdded(new[] { new Claim("type1", "value1"), new Claim("type2", "value2") });

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as ClaimsAdded;

            Assert.NotNull(eventChk);
            var evtClaims = evt.ClaimsToAdd.ToArray();
            var eventChkClaims = eventChk.ClaimsToAdd.ToArray();

            Assert.Equal(evtClaims.Length, eventChkClaims.Length);
            for (int i = 0; i < evtClaims.Length; i++)
            {
                Assert.Equal(evtClaims[i].Type, eventChkClaims[i].Type);
                Assert.Equal(evtClaims[i].Value, eventChkClaims[i].Value);
            }
        }

        [Fact]
        public void TestSerializationOfClaimsRemoved()
        {
            var evt = new ClaimsRemoved(new[] { new Claim("type1", "value1"), new Claim("type2", "value2") });

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as ClaimsRemoved;

            Assert.NotNull(eventChk);
            var evtClaims = evt.ClaimsToRemove.ToArray();
            var eventChkClaims = eventChk.ClaimsToRemove.ToArray();

            Assert.Equal(evtClaims.Length, eventChkClaims.Length);
            for (int i = 0; i < evtClaims.Length; i++)
            {
                Assert.Equal(evtClaims[i].Type, eventChkClaims[i].Type);
                Assert.Equal(evtClaims[i].Value, eventChkClaims[i].Value);
            }
        }

        [Fact]
        public void TestSerializationOfEmailChanged()
        {
            var evt = new EmailChanged("EMAILADRESS", true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as EmailChanged;

            Assert.Equal(evt.Email, eventChk.Email);
            Assert.Equal(evt.Normalized, eventChk.Normalized);
        }

        [Fact]
        public void TestSerializationOfEmailConfirmedChanged()
        {
            var evt = new EmailConfirmed(true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as EmailConfirmed;

            Assert.Equal(evt.Confirmed, eventChk.Confirmed);
        }

        [Fact]
        public void TestSerializationOfLockOutEnabled()
        {
            var evt = new LockoutEnabledChanged(true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as LockoutEnabledChanged;

            Assert.Equal(evt.LockoutEnabled, eventChk.LockoutEnabled);
        }

        [Fact]
        public void TestSerializationOfLockOutEndDateChanged()
        {
            var evt = new LockoutEndDateChanged(null);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as LockoutEndDateChanged;

            Assert.Equal(evt.LockoutEnd, eventChk.LockoutEnd);

            var dt = DateTimeOffset.UtcNow;
            evt = new LockoutEndDateChanged(dt);

            bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as LockoutEndDateChanged;

            Assert.Equal(evt.LockoutEnd, eventChk.LockoutEnd);
        }

        [Fact]
        public void TestSerializationOfLoginRemoved()
        {
            var evt = new LoginRemoved("LOGINPROVIDER", "PROVIDERKEY");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as LoginRemoved;

            Assert.Equal(evt.LoginProvider, eventChk.LoginProvider);
            Assert.Equal(evt.ProviderKey, eventChk.ProviderKey);
        }

        [Fact]
        public void TestSerializationOfPasswordHashChanged()
        {
            var evt = new PasswordHashChanged("NEWHASH");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as PasswordHashChanged;

            Assert.Equal(evt.PasswordHash, eventChk.PasswordHash);
        }

        [Fact]
        public void TestSerializationOfPhonenrChanged()
        {
            var evt = new PhoneNumberChanged("NEWHASH");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as PhoneNumberChanged;

            Assert.Equal(evt.PhoneNumber, eventChk.PhoneNumber);
        }

        [Fact]
        public void TestSerializationOfPhonenrConfirmedChanged()
        {
            var evt = new PhoneNumberConfirmed(true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as PhoneNumberConfirmed;

            Assert.Equal(evt.Confirmed, eventChk.Confirmed);
        }

        [Fact]
        public void TestSerializationOfSecurityStampChanged()
        {
            var evt = new SecurityStampChanged("STAMP");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as SecurityStampChanged;

            Assert.Equal(evt.Stamp, eventChk.Stamp);
        }

        [Fact]
        public void TestSerializationOfTokenAdded()
        {
            var evt = new TokenAdded("LOGINPROVIDER", "NAME", "VALUE");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as TokenAdded;

            Assert.Equal(evt.LoginProvider, eventChk.LoginProvider);
            Assert.Equal(evt.Name, eventChk.Name);
            Assert.Equal(evt.Value, eventChk.Value);
        }

        [Fact]
        public void TestSerializationOfTokenRemoved()
        {
            var evt = new TokenRemoved("LOGINPROVIDER", "NAME");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as TokenRemoved;

            Assert.Equal(evt.LoginProvider, eventChk.LoginProvider);
            Assert.Equal(evt.Name, eventChk.Name);
        }

        [Fact]
        public void TestSerializationOfTokenUpdated()
        {
            var evt = new TokenUpdated("LOGINPROVIDER", "NAME", "VALUE");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as TokenUpdated;

            Assert.Equal(evt.LoginProvider, eventChk.LoginProvider);
            Assert.Equal(evt.Name, eventChk.Name);
            Assert.Equal(evt.Value, eventChk.Value);
        }

        [Fact]
        public void TestSerializationOfTwoFactorChanged()
        {
            var evt = new TwoFactorEnabledChanged(true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as TwoFactorEnabledChanged;

            Assert.Equal(evt.TwoFactorEnabled, eventChk.TwoFactorEnabled);
        }

        [Fact]
        public void TestSerializationOfUsersClaimsAdded()
        {
            var userId = Guid.NewGuid();
            var evt = new UserClaimsAdded<Guid>(userId, new[] { new Claim("type1", "value1"), new Claim("type2", "value2") });

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserClaimsAdded<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);

            var evtClaims = evt.Claims.ToArray();
            var eventChkClaims = eventChk.Claims.ToArray();

            Assert.Equal(evtClaims.Length, eventChkClaims.Length);
            for (int i = 0; i < evtClaims.Length; i++)
            {
                Assert.Equal(evtClaims[i].Type, eventChkClaims[i].Type);
                Assert.Equal(evtClaims[i].Value, eventChkClaims[i].Value);
            }
        }

        [Fact]
        public void TestSerializationOfUsersClaimsRemoved()
        {
            var userId = Guid.NewGuid();
            var evt = new UserClaimsRemoved<Guid>(userId, new[] { new Claim("type1", "value1"), new Claim("type2", "value2") });

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserClaimsRemoved<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);

            var evtClaims = evt.Claims.ToArray();
            var eventChkClaims = eventChk.Claims.ToArray();

            Assert.Equal(evtClaims.Length, eventChkClaims.Length);
            for (int i = 0; i < evtClaims.Length; i++)
            {
                Assert.Equal(evtClaims[i].Type, eventChkClaims[i].Type);
                Assert.Equal(evtClaims[i].Value, eventChkClaims[i].Value);
            }
        }

        [Fact]
        public void TestSerializationOfUSerCreated2()
        {
            var usr = new TestIdentityUser(Guid.NewGuid())
            {
                AccessFailedCount = 5,
                Claims = new[] { new Claim("type1", "value1"), new Claim("type2", "value2") },
                Email = "USEREMAIL",
                EmailConfirmed = true,
                LockoutEnabled = false,
                LockoutEnd = null,
                Logins = new List<ImmutableUserLoginInfo> { new ImmutableUserLoginInfo("LOGINPROVIDER1", "PROVIDERKEY1", "DISPLAYNAME1"), new ImmutableUserLoginInfo("LOGINPROVIDER2", "PROVIDERKEY2", "DISPLAYNAME2") },
                NormalizedEmail = "NORMALIZEDEMAIL",
                NormalizedUserName = "NORMALIZEDUSERNAME",
                PasswordHash = null,
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                SecurityStamp = "STAMP",
                Tokens = null,
                TwoFactorEnabled = false,
                UserName = "USERNAME"
            };

            var evt = new UserCreated<Guid, TestIdentityUser>(usr);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserCreated<Guid, TestIdentityUser>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.User.AccessFailedCount, eventChk.User.AccessFailedCount);
            Assert.Equal(evt.User.Email, eventChk.User.Email);
            Assert.Equal(evt.User.EmailConfirmed, eventChk.User.EmailConfirmed);
            Assert.Equal(evt.User.LockoutEnabled, eventChk.User.LockoutEnabled);
            Assert.Equal(evt.User.LockoutEnd, eventChk.User.LockoutEnd);
            Assert.Equal(evt.User.NormalizedEmail, eventChk.User.NormalizedEmail);
            Assert.Equal(evt.User.NormalizedUserName, eventChk.User.NormalizedUserName);
            Assert.Equal(evt.User.PasswordHash, eventChk.User.PasswordHash);
            Assert.Equal(evt.User.PhoneNumber, eventChk.User.PhoneNumber);
            Assert.Equal(evt.User.SecurityStamp, eventChk.User.SecurityStamp);
            Assert.Equal(evt.User.UserName, eventChk.User.UserName);
        }

        [Fact]
        public void TestSerializationOfUserDeleted1()
        {
            var userId = Guid.NewGuid();
            var evt = new UserDeleted<Guid>(userId);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserDeleted<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);
        }

        [Fact]
        public void TestSerializationOfUserDeleted()
        {
            var evt = new UserDeleted();

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserDeleted;

            Assert.NotNull(eventChk);
        }

        [Fact]
        public void TestSerializationOfUserEmailChanged()
        {
            var userId = Guid.NewGuid();
            var evt = new UserEmailChanged<Guid>(userId, "EMAIL");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserEmailChanged<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);
            Assert.Equal(evt.Email, eventChk.Email);
        }

        [Fact]
        public void TestSerializationOfUserLoginAdded()
        {
            var userId = Guid.NewGuid();
            var evt = new UserLoginAdded<Guid>(userId, new ImmutableUserLoginInfo("LOGINPROVIDER", "PROVIDERKEY", "DISPLAYNAME"));

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserLoginAdded<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);
            Assert.Equal(evt.UserLoginInfo.LoginProvider, eventChk.UserLoginInfo.LoginProvider);
            Assert.Equal(evt.UserLoginInfo.ProviderKey, eventChk.UserLoginInfo.ProviderKey);
            Assert.Equal(evt.UserLoginInfo.DisplayName, eventChk.UserLoginInfo.DisplayName);
        }

        [Fact]
        public void TestSerializationOfUserLoginInfoAdded()
        {
            var evt = new UserLoginInfoAdded(new ImmutableUserLoginInfo("LOGINPROVIDER", "PROVIDERKEY", "DISPLAYNAME"));

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserLoginInfoAdded;

            Assert.NotNull(eventChk);
            
            Assert.Equal(evt.UserloginInfo.LoginProvider, eventChk.UserloginInfo.LoginProvider);
            Assert.Equal(evt.UserloginInfo.ProviderKey, eventChk.UserloginInfo.ProviderKey);
            Assert.Equal(evt.UserloginInfo.DisplayName, eventChk.UserloginInfo.DisplayName);
        }

        [Fact]
        public void TestSerializationOfUserLoginInfoRemoved()
        {
            var userId = Guid.NewGuid();
            var evt = new UserLoginRemoved<Guid>(userId, new ImmutableUserLoginInfo("LOGINPROVIDER", "PROVIDERKEY", "DISPLAYNAME"));

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserLoginRemoved<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);
            Assert.Equal(evt.UserLoginInfo.LoginProvider, eventChk.UserLoginInfo.LoginProvider);
            Assert.Equal(evt.UserLoginInfo.ProviderKey, eventChk.UserLoginInfo.ProviderKey);
            Assert.Equal(evt.UserLoginInfo.DisplayName, eventChk.UserLoginInfo.DisplayName);
        }

        [Fact]
        public void TestSerializationOfUserNameChanged1()
        {
            var userId = Guid.NewGuid();
            var evt = new UserNameChanged<Guid>(userId, "EMAIL");

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserNameChanged<Guid>;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserId, eventChk.UserId);
            Assert.Equal(evt.UserName, eventChk.UserName);
        }

        [Fact]
        public void TestSerializationOfUserNameChanged()
        {
            var evt = new UserNameChanged( "EMAIL", true);

            var bytes = LZ4MessagePackSerializer.Serialize<IEvent>(evt, AspnetIdentityResolver.Instance);
            var eventChk = LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance) as UserNameChanged;

            Assert.NotNull(eventChk);
            Assert.Equal(evt.UserName, eventChk.UserName);
            Assert.Equal(evt.Normalized, eventChk.Normalized);
        }
    }
}
