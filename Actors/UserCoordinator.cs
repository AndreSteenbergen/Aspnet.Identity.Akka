using Akka.Actor;
using Aspnet.Identity.Akka.ActorHelpers;
using Aspnet.Identity.Akka.ActorMessages;
using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.Actors
{
    /// <summary>
    /// non persistent usercoordinator
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public class UserCoordinator<TKey, TUser> : UntypedActor
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        private UserCoordinatorHelper<TKey, TUser> userCoordinatorHelper;
        private bool coordinatorInsync;

        private Action<IEvent, Action<IEvent>> persist;
        private readonly Action<TKey, IEvent, Action<IEvent>> childPersist;

        public UserCoordinator(
            Action<IEvent, Action<IEvent>> coordinatorPerist,
            Action<TKey, IEvent, Action<IEvent>> childPersist)
        {
            this.persist = coordinatorPerist;
            this.childPersist = childPersist;

            var ctx = Context;
            userCoordinatorHelper = new UserCoordinatorHelper<TKey, TUser>(CreateChildActor, () => ctx);
        }

        private IActorRef CreateChildActor(TKey identityUser)
        {
            var childActor = Context.ActorOf(Props.Create(() => new UserActor<TKey, TUser>(identityUser, Context.Self, childPersist)));
            if (coordinatorInsync)
            {
                childActor.Tell(new InSyncCommand(true));
            }
            return childActor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case InSyncCommand insync:
                    coordinatorInsync = true;
                    userCoordinatorHelper.SetInSync(insync.IsSynchronized);
                    break;
                case IEvent @event:
                    userCoordinatorHelper.OnEvent(Sender, @event);
                    break;
                case ICommand command:
                    userCoordinatorHelper.OnCommand(Sender, command, persist);
                    break;
            }
        }
    }
}
