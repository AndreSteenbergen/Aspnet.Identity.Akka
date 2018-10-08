using Akka.Actor;
using Akka.Persistence;
using Aspnet.Identity.Akka.ActorHelpers;
using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.Actors
{
    public class PersistentUserCoordinator<TKey, TUser> : UntypedPersistentActor
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private string persistenceId;
        private UserCoordinatorHelper<TKey, TUser> userCoordinatorHelper;

        public override string PersistenceId => persistenceId;

        public PersistentUserCoordinator(string persistenceId)
        {
            this.persistenceId = persistenceId;
            var ctx = Context;
            userCoordinatorHelper = new UserCoordinatorHelper<TKey, TUser>(CreateChildActor, () => ctx);
        }

        private IActorRef CreateChildActor(TKey arg)
        {
            return Context.ActorOf(Props.Create(() => new PersistentUserActor<TKey, TUser>(arg, Context.Self, persistenceId)));
        }

        protected override void OnReplaySuccess()
        {
            base.OnReplaySuccess();
            userCoordinatorHelper.SetInSync(false);
        }

        protected override void OnCommand(object message)
        {
            if (message is ICommand)
                userCoordinatorHelper.OnCommand(Sender, message as ICommand, Persist);
        }

        protected override void OnRecover(object message)
        {
            if (message is IEvent)
                userCoordinatorHelper.OnEvent(Sender, message as IEvent);
        }
    }
}
