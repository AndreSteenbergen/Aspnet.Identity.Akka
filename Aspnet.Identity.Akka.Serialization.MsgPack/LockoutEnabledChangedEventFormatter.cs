using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class LockoutEnabledChangedEventFormatter : IMessagePackFormatter<LockoutEnabledChanged>
    {
        public LockoutEnabledChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var b = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            return new LockoutEnabledChanged(b);
        }

        public int Serialize(ref byte[] bytes, int offset, LockoutEnabledChanged value, IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteBoolean(ref bytes, offset, value.LockoutEnabled);
        }
    }

}
