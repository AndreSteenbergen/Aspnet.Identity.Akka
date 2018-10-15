using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class LoginRemovedEventFormatter : IMessagePackFormatter<LoginRemoved>
    {
        public LoginRemoved Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var s = MessagePackBinary.ReadString(bytes, offset, out int stringSize);
            var b = MessagePackBinary.ReadString(bytes, offset + stringSize, out int stringSize2);
            readSize = stringSize + stringSize2;
            return new LoginRemoved(s, b);
        }

        public int Serialize(ref byte[] bytes, int offset, LoginRemoved value, IFormatterResolver formatterResolver)
        {
            var result = MessagePackBinary.WriteString(ref bytes, offset, value.LoginProvider);
            return result + MessagePackBinary.WriteString(ref bytes, offset + result, value.ProviderKey);
        }
    }
}
