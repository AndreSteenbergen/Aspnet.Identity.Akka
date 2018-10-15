using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;
using System;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserDeletedEventFormatter<TKey> : IMessagePackFormatter<UserDeleted<TKey>>
        where TKey : IEquatable<TKey>
    {
        public UserDeleted<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var formatter = formatterResolver.GetFormatter<TKey>();
            var k = formatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            return new UserDeleted<TKey>(k);
        }

        public int Serialize(ref byte[] bytes, int offset, UserDeleted<TKey> value, IFormatterResolver formatterResolver)
        {
            var formatter = formatterResolver.GetFormatter<TKey>();
            return formatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
        }
    }
}
