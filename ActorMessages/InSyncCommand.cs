using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.ActorMessages
{
    public class InSyncCommand : ICommand
    {
        public InSyncCommand(bool inSync)
        {
            IsSynchronized = inSync;
        }
        public bool IsSynchronized { get; }
    }

    public class InSyncCommand<TKey> : InSyncCommand, ICommand<TKey> where TKey : IEquatable<TKey>
    {
        public InSyncCommand(TKey key, bool inSync) : base(inSync)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}
