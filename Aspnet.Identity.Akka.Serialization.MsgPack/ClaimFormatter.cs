using MessagePack.Formatters;
using MessagePack;
using System.Security.Claims;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class ClaimFormatter : IMessagePackFormatter<Claim>
    {
        public Claim Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            int readOffset = offset;
            int readsize;
            var t = MessagePackBinary.ReadString(bytes, offset, out readsize);
            offset += readsize;

            var v = MessagePackBinary.ReadString(bytes, offset, out readsize);
            offset += readsize;

            readSize = offset - readOffset;
            return new Claim(t, v);
        }

        public int Serialize(ref byte[] bytes, int offset, Claim value, IFormatterResolver formatterResolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.Type);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.Value);
            return offset - startOffset;
        }
    }
}
