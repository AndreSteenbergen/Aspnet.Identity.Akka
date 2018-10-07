using System;

namespace Aspnet.Identity.Akka.Interfaces
{
    public interface ICommand
    {
    }

    public interface ICommand<TKey> : ICommand where TKey : IEquatable<TKey>
    {
        TKey Key { get; }
    }
}
