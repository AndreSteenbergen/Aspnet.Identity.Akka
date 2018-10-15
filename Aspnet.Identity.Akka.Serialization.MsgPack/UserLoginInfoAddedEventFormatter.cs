using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class UserLoginInfoAddedEventFormatter : IMessagePackFormatter<UserLoginInfoAdded>
    {
        public UserLoginInfoAdded Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var startOffset = offset;

            var loginProvider = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var providerKey = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var displayName = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;

            readSize = offset - startOffset;
            return new UserLoginInfoAdded(new Model.ImmutableUserLoginInfo(loginProvider, providerKey, displayName));
        }

        public int Serialize(ref byte[] bytes, int offset, UserLoginInfoAdded value, IFormatterResolver formatterResolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserloginInfo.LoginProvider);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserloginInfo.ProviderKey);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.UserloginInfo.DisplayName);
            return offset - startOffset;
        }
    }
}
