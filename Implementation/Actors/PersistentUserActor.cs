using Akka.Actor;
using Akka.Persistence;
using Aspnet.Identity.Akka.ActorHelpers;
using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.Actors
{
    public class PersistentUserActor<TKey, TUser> : UntypedPersistentActor
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private readonly TKey userId;
        private readonly string persistenceId;
        private readonly UserHelper<TKey, TUser> userHelper;

        public PersistentUserActor(TKey userId, IActorRef coordinator, string persistenceId)
        {
            this.userId = userId;
            this.persistenceId = persistenceId;
            userHelper = new UserHelper<TKey, TUser>(userId, coordinator);
        }

        public override string PersistenceId => $"{persistenceId}-{userId}-{userId.GetHashCode()}";

        protected override void OnReplaySuccess()
        {
            base.OnReplaySuccess();
            userHelper.SetInSync();
        }

        protected override void OnCommand(object message)
        {
            if (message is ICommand)
                userHelper.OnCommand(Sender, message as ICommand, Persist);
        }

        protected override void OnRecover(object message)
        {
            if (message is IEvent)
                userHelper.OnEvent(Sender, message as IEvent);
        }
    }
}
