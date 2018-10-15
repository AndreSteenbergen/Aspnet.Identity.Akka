using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class TokenUpdatedEventFormatter : IMessagePackFormatter<TokenUpdated>
    {
        public TokenUpdated Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var s1 = MessagePackBinary.ReadString(bytes, offset, out int stringSize);
            var s2 = MessagePackBinary.ReadString(bytes, offset + stringSize, out int stringSize2);
            var s3 = MessagePackBinary.ReadString(bytes, offset + stringSize + stringSize2, out int stringSize3);
            readSize = stringSize + stringSize2 + stringSize3;
            return new TokenUpdated(s1, s2, s3);
        }

        public int Serialize(ref byte[] bytes, int offset, TokenUpdated value, IFormatterResolver formatterResolver)
        {
            var result = MessagePackBinary.WriteString(ref bytes, offset, value.LoginProvider);
            result += MessagePackBinary.WriteString(ref bytes, offset + result, value.Name);
            return result + MessagePackBinary.WriteString(ref bytes, offset + result, value.Value);
        }
    }
    
}
