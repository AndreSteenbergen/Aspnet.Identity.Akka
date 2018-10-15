using MessagePack.Formatters;
using Aspnet.Identity.Akka.ActorMessages.User;
using System;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class LockoutEndDateChangedEventFormatter : IMessagePackFormatter<LockoutEndDateChanged>
    {
        public LockoutEndDateChanged Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return new LockoutEndDateChanged(null);
            }
            var dt = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
            return new LockoutEndDateChanged(new DateTimeOffset(dt));
        }

        public int Serialize(ref byte[] bytes, int offset, LockoutEndDateChanged value, IFormatterResolver formatterResolver)
        {
            if (value.LockoutEnd == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            return MessagePackBinary.WriteDateTime(ref bytes, offset, value.LockoutEnd.Value.UtcDateTime);
        }
    }

}
