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
        private readonly TUser identityUser;
        private readonly string persistenceId;
        private readonly UserHelper<TKey, TUser> userHelper;

        public PersistentUserActor(TUser identityUser, string persistenceId)
        {
            this.identityUser = identityUser;
            this.persistenceId = persistenceId;
            userHelper = new UserHelper<TKey, TUser>(identityUser);
        }

        public override string PersistenceId => $"{persistenceId}-{identityUser.Id}-{identityUser.Id.GetHashCode()}";

        protected override void OnReplaySuccess()
        {
            base.OnReplaySuccess();
            userHelper.SetInSync(true);
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
