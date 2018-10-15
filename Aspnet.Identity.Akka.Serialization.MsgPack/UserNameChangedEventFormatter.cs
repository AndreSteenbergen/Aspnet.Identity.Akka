using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserNameChangedEventFormatter : IMessagePackFormatter<UserNameChanged>
    {
        public UserNameChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var startOffset = offset;
            var uName = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var norm = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            offset += readSize;
            readSize = offset - startOffset;

            return new UserNameChanged(uName, norm);
        }

        public int Serialize(ref byte[] bytes, int offset, UserNameChanged value, IFormatterResolver formatterResolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserName);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Normalized);
            return offset - startOffset;
        }
    }
}
