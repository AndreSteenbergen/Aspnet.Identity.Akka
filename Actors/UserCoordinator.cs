using Akka.Actor;
using Aspnet.Identity.Akka.ActorHelpers;
using Aspnet.Identity.Akka.ActorMessages;
using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.Actors
{
    public class UserCoordinator<TKey, TUser> : UntypedActor
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private UserCoordinatorHelper<TKey, TUser> userCoordinatorHelper;

        public UserCoordinator()
        {
            userCoordinatorHelper = new UserCoordinatorHelper<TKey, TUser>(CreateChildActor, KillChildActor);
        }

        private void KillChildActor(IActorRef child)
        {
            Context.Stop(child);
        }

        private IActorRef CreateChildActor(TKey identityUser)
        {
            return Context.ActorOf(Props.Create(() => new UserActor<TKey, TUser>(identityUser, Context.Self)));
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case InSyncCommand insync:
                    userCoordinatorHelper.SetInSync(insync.IsSynchronized);
                    break;
                case IEvent @event:
                    userCoordinatorHelper.OnEvent(Sender, @event);
                    break;
                case ICommand command:
                    userCoordinatorHelper.OnCommand(Sender, command, (evt, a) => a(evt));
                    break;
            }
        }
    }
}
