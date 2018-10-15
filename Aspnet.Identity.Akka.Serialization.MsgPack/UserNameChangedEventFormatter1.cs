using MessagePack.Formatters;
using System;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserNameChangedEventFormatter<TKey> : IMessagePackFormatter<UserNameChanged<TKey>>
    where TKey : IEquatable<TKey>
    {
        public UserNameChanged<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();

            var startOffset = offset;
            var key = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            offset += readSize;
            var username = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;

            readSize = offset - startOffset;

            return new UserNameChanged<TKey>(key, username);
        }

        public int Serialize(ref byte[] bytes, int offset, UserNameChanged<TKey> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var startOffset = offset;
            offset += keyFormatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserName);
            return offset - startOffset;
        }
    }
}
