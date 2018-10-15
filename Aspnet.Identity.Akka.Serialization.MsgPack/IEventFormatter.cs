using Aspnet.Identity.Akka.Interfaces;
using MessagePack;
using MessagePack.Formatters;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class IEventFormatter : IMessagePackFormatter<IEvent>
    {
        private readonly TypelessFormatter typelessFormatter;

        public IEventFormatter()
        {
            typelessFormatter = new TypelessFormatter();
        }

        public IEvent Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            return typelessFormatter.Deserialize(bytes, offset, formatterResolver, out readSize) as IEvent;
        }

        public int Serialize(ref byte[] bytes, int offset, IEvent value, IFormatterResolver formatterResolver)
        {
            return typelessFormatter.Serialize(ref bytes, offset, value, formatterResolver);
        }
    }
}
