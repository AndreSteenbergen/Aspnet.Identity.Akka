using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using System;
using MessagePack;
using System.Security.Claims;
using System.Collections.Generic;
using Aspnet.Identity.Akka.Model;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserCreatedEventFormatter<TKey, TUser> : IMessagePackFormatter<UserCreated<TKey, TUser>>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        public UserCreated<TKey, TUser> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var claimFormatter = formatterResolver.GetFormatter<Claim>();

            var startOffset = offset;

            var key = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            var usr = (TUser)Activator.CreateInstance(typeof(TUser), key);
            offset += readSize;
            usr.UserName = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.NormalizedUserName = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.Email = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.NormalizedEmail = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.EmailConfirmed = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            offset += readSize;
            usr.PasswordHash = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.SecurityStamp = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.PhoneNumber = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            usr.PhoneNumberConfirmed = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            offset += readSize;
            usr.TwoFactorEnabled = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            offset += readSize;
            usr.LockoutEnd = null;
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
            }
            else
            {
                usr.LockoutEnd = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
                offset += readSize;
            }
            usr.LockoutEnabled = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            offset += readSize;
            usr.AccessFailedCount = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            offset += readSize;

            usr.Claims = null;
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                usr.Claims = new List<Claim>(len);
                for (int i = 0; i < len; i++)
                {
                    var claim = claimFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    usr.Claims.Add(claim);
                }
            }

            usr.Logins = null;
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                usr.Logins = new List<ImmutableUserLoginInfo>(len);
                for (int i = 0; i < len; i++)
                {
                    var loginProvider = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                    var providerKey = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                    var displayName = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;

                    usr.Logins.Add(new ImmutableUserLoginInfo(loginProvider, providerKey, displayName));                    
                }
            }

            usr.Tokens = null;
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                usr.Tokens = new List<ImmutableIdentityUserToken>(len);
                for (int i = 0; i < len; i++)
                {
                    var loginProvider = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                    var name = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                    var value = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;

                    usr.Tokens.Add(new ImmutableIdentityUserToken(loginProvider, name, value));
                }
            }
            readSize = offset - startOffset;
            return new UserCreated<TKey, TUser>(usr);
        }

        public int Serialize(ref byte[] bytes, int offset, UserCreated<TKey, TUser> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var claimFormatter = formatterResolver.GetFormatter<Claim>();

            var startOffset = offset;
            offset += keyFormatter.Serialize(ref bytes, offset, value.User.Id, formatterResolver);

            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.UserName);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.NormalizedUserName);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.Email);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.NormalizedEmail);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.User.EmailConfirmed);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.PasswordHash);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.SecurityStamp);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.User.PhoneNumber);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.User.PhoneNumberConfirmed);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.User.TwoFactorEnabled);
            if (value.User.LockoutEnd == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteDateTime(ref bytes, offset, value.User.LockoutEnd.Value.UtcDateTime);
            }
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.User.LockoutEnabled);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.User.AccessFailedCount);

            if (value.User.Claims == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.User.Claims.Count);
                for (int i = 0; i < value.User.Claims.Count; i++)
                {
                    offset += claimFormatter.Serialize(ref bytes, offset, value.User.Claims[i], formatterResolver);
                }
            }

            if (value.User.Logins == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.User.Logins.Count);
                for (int i = 0; i < value.User.Logins.Count; i++)
                {
                    var login = value.User.Logins[i];
                    offset += MessagePackBinary.WriteString(ref bytes, offset, login.LoginProvider);
                    offset += MessagePackBinary.WriteString(ref bytes, offset, login.ProviderKey);
                    offset += MessagePackBinary.WriteString(ref bytes, offset, login.DisplayName);
                }
            }

            if (value.User.Tokens == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.User.Tokens.Count);
                for (int i = 0; i < value.User.Tokens.Count; i++)
                {
                    var token = value.User.Tokens[i];
                    offset += MessagePackBinary.WriteString(ref bytes, offset, token.LoginProvider);
                    offset += MessagePackBinary.WriteString(ref bytes, offset, token.Name);
                    offset += MessagePackBinary.WriteString(ref bytes, offset, token.Value);
                }
            }
            return offset - startOffset;
        }
    }
}
