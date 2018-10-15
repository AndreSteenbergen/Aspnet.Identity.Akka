using System;
using Akka.Actor;
using Akka.Serialization;
using Aspnet.Identity.Akka.Interfaces;
using MessagePack;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class AspnetIdentitySerializer : Serializer
    {
        protected AspnetIdentitySerializer(ExtendedActorSystem system) : base(system)
        {
            
        }

        public override bool IncludeManifest => false;

        public override object FromBinary(byte[] bytes, Type type)
        {
            try
            {
                return LZ4MessagePackSerializer.Deserialize<IEvent>(bytes, AspnetIdentityResolver.Instance);
            } catch
            {
                return null;
            }
        }

        public override byte[] ToBinary(object obj)
        {
            if (!(obj is IEvent))
                throw new ArgumentException("Only IEvents can be used with this serializer");
            return LZ4MessagePackSerializer.Serialize((IEvent) obj, AspnetIdentityResolver.Instance);
        }

        public override int Identifier => 98;
    }
}
