using MessagePack.Formatters;
using System;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserCreatedEventFormatter<TKey> : IMessagePackFormatter<UserCreated<TKey>>
        where TKey : IEquatable<TKey>
    {
        public UserCreated<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();

            var startOffset = offset;
            var key = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            offset += readSize;
            var name = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var pw = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            readSize = offset - startOffset;

            return new UserCreated<TKey>(key, name, pw);
        }

        public int Serialize(ref byte[] bytes, int offset, UserCreated<TKey> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var startOffset = offset;
            offset += keyFormatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.NormalizedUserName);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.NormalizedEmail);
            return offset - startOffset;
        }
    }

}
