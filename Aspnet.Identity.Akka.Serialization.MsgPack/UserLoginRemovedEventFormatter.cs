using MessagePack.Formatters;
using System;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserLoginRemovedEventFormatter<TKey> : IMessagePackFormatter<UserLoginRemoved<TKey>>
    where TKey : IEquatable<TKey>
    {
        public UserLoginRemoved<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();

            var startOffset = offset;
            var key = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            offset += readSize;
            var loginProvider = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var providerKey = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var displayName = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            readSize = offset - startOffset;

            return new UserLoginRemoved<TKey>(key, new Model.ImmutableUserLoginInfo(loginProvider, providerKey, displayName));
        }

        public int Serialize(ref byte[] bytes, int offset, UserLoginRemoved<TKey> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var startOffset = offset;
            offset += keyFormatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserLoginInfo.LoginProvider);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserLoginInfo.ProviderKey);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserLoginInfo.DisplayName);
            return offset - startOffset;
        }
    }
}
