using Akka.Actor;
using Aspnet.Identity.Akka.ActorHelpers;
using Aspnet.Identity.Akka.ActorMessages;
using Aspnet.Identity.Akka.Interfaces;
using System;

namespace Aspnet.Identity.Akka.Actors
{
    /// <summary>
    /// non persistent usercoordinator, provides callbacks to actions so persistence can occur outside of the actors
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

        /// <summary>
        /// Constructor for streaming usercoordinator
        /// </summary>
        /// <param name="startAsInSync"></param>
        /// <param name="coordinatorPerist"></param>
        /// <param name="childPersist">Important to store objects of type IUserEvent<TKey>, coordinator needs to send events to correct userActor</param>
        public UserCoordinator(
            bool startAsInSync,
            Action<IEvent, Action<IEvent>> coordinatorPerist,
            Action<TKey, IEvent, Action<IEvent>> childPersist)
        {
            coordinatorInsync = startAsInSync;
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
                childActor.Tell(InSyncCommand.Instance);
            }
            return childActor;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case InSyncCommand insync:
                    coordinatorInsync = true;
                    userCoordinatorHelper.SetInSync(true);
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
