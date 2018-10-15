using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class TokenRemovedEventFormatter : IMessagePackFormatter<TokenRemoved>
    {
        public TokenRemoved Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var startOffset = offset;
            var loginProvider = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var name = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            readSize = offset - startOffset;

            return new TokenRemoved(loginProvider, name);
        }

        public int Serialize(ref byte[] bytes, int offset, TokenRemoved value, IFormatterResolver formatterResolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.LoginProvider);
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.Name);
            return offset - startOffset;
        }
    }
}
