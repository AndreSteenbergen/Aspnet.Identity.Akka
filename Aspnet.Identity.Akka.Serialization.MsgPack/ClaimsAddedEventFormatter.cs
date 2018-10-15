using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class ClaimsAddedEventFormatter : IMessagePackFormatter<ClaimsAdded>
    {
        public ClaimsAdded Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var claimFormatter = formatterResolver.GetFormatter<Claim>();
            var startOffset = offset;

            var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;
            var array = new Claim[len];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = claimFormatter.Deserialize(bytes, offset, formatterResolver, out readSize);
                offset += readSize;
            }
            readSize = offset - startOffset;
            return new ClaimsAdded(array);
        }

        public int Serialize(ref byte[] bytes, int offset, ClaimsAdded value, IFormatterResolver formatterResolver)
        {
            var claimFormatter = formatterResolver.GetFormatter<Claim>();
            var startOffset = offset;
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.ClaimsToAdd.Length);
            for (int i = 0; i < value.ClaimsToAdd.Length; i++)
            {
                var claim = value.ClaimsToAdd[i];
                offset += claimFormatter.Serialize(ref bytes, offset, claim, formatterResolver);
            }
            return offset - startOffset;
        }
    }
    
}
