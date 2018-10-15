using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class PhoneNumberChangedEventFormatter : IMessagePackFormatter<PhoneNumberChanged>
    {
        public PhoneNumberChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var b = MessagePackBinary.ReadString(bytes, offset, out readSize);
            return new PhoneNumberChanged(b);
        }

        public int Serialize(ref byte[] bytes, int offset, PhoneNumberChanged value, IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteString(ref bytes, offset, value.PhoneNumber);
        }
    }

}
