using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class EmailChangedEventFormatter : IMessagePackFormatter<EmailChanged> {
        public EmailChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var s = MessagePackBinary.ReadString(bytes, offset, out int stringSize);
            var b = MessagePackBinary.ReadBoolean(bytes, offset + stringSize, out int boolSize);
            readSize = stringSize + boolSize;
            return new EmailChanged(s, b);
        }

        public int Serialize(ref byte[] bytes, int offset, EmailChanged value, IFormatterResolver formatterResolver)
        {
            var result = MessagePackBinary.WriteString(ref bytes, offset, value.Email);
            return result + MessagePackBinary.WriteBoolean(ref bytes, offset + result, value.Normalized);
        }
    }
}
