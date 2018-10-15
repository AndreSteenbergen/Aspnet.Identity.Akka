using MessagePack.Formatters;
using MessagePack;
using System.Security.Claims;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using System;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserClaimsRemovedEventFormatter<TKey> : IMessagePackFormatter<UserClaimsRemoved<TKey>>
        where TKey : IEquatable<TKey>
    {
        public UserClaimsRemoved<TKey> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var claimFormatter = formatterResolver.GetFormatter<Claim>();
            var keyFormatter = formatterResolver.GetFormatter<TKey>();

            var startOffset = offset;
            var k = keyFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
            offset += readSize;

            var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;
            var array = new Claim[len];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = claimFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
                offset += readSize;
            }
            readSize = offset - startOffset;
            return new UserClaimsRemoved<TKey>(k, array);
        }

        public int Serialize(ref byte[] bytes, int offset, UserClaimsRemoved<TKey> value, IFormatterResolver formatterResolver)
        {
            var keyFormatter = formatterResolver.GetFormatter<TKey>();
            var claimFormatter = formatterResolver.GetFormatter<Claim>();

            var startOffset = offset;

            offset += keyFormatter.Serialize(ref bytes, offset, value.UserId, formatterResolver);
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Claims.Length);
            for (int i = 0; i < value.Claims.Length; i++)
            {
                var claim = value.Claims[i];
                offset += claimFormatter.Serialize(ref bytes, offset, claim, formatterResolver);
            }
            return offset - startOffset;
        }
    }
    
}
