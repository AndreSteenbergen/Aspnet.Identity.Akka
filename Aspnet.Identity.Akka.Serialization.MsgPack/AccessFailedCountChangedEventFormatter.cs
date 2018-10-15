using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class AccessFailedCountChangedEventFormatter : IMessagePackFormatter<AccessFailedCountChanged>
    {
        public AccessFailedCountChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var i = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            return new AccessFailedCountChanged(i);
        }

        public int Serialize(ref byte[] bytes, int offset, AccessFailedCountChanged value, IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, value.AccessFailedCount);
        }
    }
}
