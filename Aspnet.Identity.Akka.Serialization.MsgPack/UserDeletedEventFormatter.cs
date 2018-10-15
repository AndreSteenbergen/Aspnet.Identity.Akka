using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserDeletedEventFormatter : IMessagePackFormatter<UserDeleted>
    {
        public UserDeleted Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            readSize = 1;
            return new UserDeleted();
        }

        public int Serialize(ref byte[] bytes, int offset, UserDeleted value, IFormatterResolver formatterResolver)
        {
            return MessagePackBinary.WriteNil(ref bytes, offset);
        }
    }
}
