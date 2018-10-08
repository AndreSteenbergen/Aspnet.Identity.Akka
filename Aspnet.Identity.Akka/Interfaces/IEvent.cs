using System;

namespace Aspnet.Identity.Akka.Interfaces
{
    public interface IEvent
    {
    }

    public interface IEvent<TKey> : IEvent where TKey : IEquatable<TKey>
    {
        TKey Key { get; }
    }
}
