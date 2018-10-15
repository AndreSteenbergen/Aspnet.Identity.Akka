using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class SecurityStampChangedEventFormatter : IMessagePackFormatter<SecurityStampChanged>
    {
        public SecurityStampChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var b = MessagePackBinary.ReadString(bytes, offset, out readSize);
            return new SecurityStampChanged(b);
        }

        public int Serialize(ref byte[] bytes, int offset, SecurityStampChanged value, IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteString(ref bytes, offset, value.Stamp);
        }
    }
}
