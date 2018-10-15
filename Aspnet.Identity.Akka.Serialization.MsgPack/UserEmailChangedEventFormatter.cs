using MessagePack.Formatters;
using System;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserEmailChangedEventFormatter<TKey> : IMessagePackFormatter<UserEmailChanged<TKey>>
    where TKey : IEquatable<TKey>
    {
        public UserEmailChanged<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();

            var startOffset = offset;
            var key = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            offset += readSize;
            var email = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            readSize = offset - startOffset;

            return new UserEmailChanged<TKey>(key, email);
        }

        public int Serialize(ref byte[] bytes, int offset, UserEmailChanged<TKey> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var startOffset = offset;
            offset += keyFormatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.Email);
            return offset - startOffset;
        }
    }

}
