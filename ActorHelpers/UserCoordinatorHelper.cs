using Akka.Actor;
using Aspnet.Identity.Akka.ActorMessages;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
using Aspnet.Identity.Akka.Actors;
using Aspnet.Identity.Akka.Interfaces;
using Aspnet.Identity.Akka.Model;
using System;
using System.Collections.Generic;

namespace Aspnet.Identity.Akka.ActorHelpers
{
    public class UserCoordinatorHelper<TKey, TUser>
        where TKey : IEquatable<TKey>
        where TUser : IdentityUser<TKey>
    {
        public UserCoordinatorHelper(Func<TKey, IActorRef> createUserActor, Func<IActorContext> getContext)
        {
            this.createUserActor = createUserActor;
            this.getContext = getContext;
        }

        private bool inSync;

        //useractors, as created by the real user coordinators
        private Dictionary<TKey, IActorRef> userActors = new Dictionary<TKey, IActorRef>();

        //just to find users by key, username, email and external login.
        private Dictionary<TKey, Tuple<string, string, HashSet<ExternalLogin>>> existingUsers = new Dictionary<TKey, Tuple<string, string, HashSet<ExternalLogin>>>();
        private Dictionary<string, TKey> existingIds = new Dictionary<string, TKey>();
        private Dictionary<string, TKey> existingUserNames = new Dictionary<string, TKey>();
        private Dictionary<string, TKey> existingEmails = new Dictionary<string, TKey>();
        private Dictionary<ExternalLogin, TKey> existingLogins = new Dictionary<ExternalLogin, TKey>();
        private Dictionary<string, Dictionary<string, HashSet<TKey>>> usersByClaims = new Dictionary<string, Dictionary<string, HashSet<TKey>>>();

        private readonly Func<TKey, IActorRef> createUserActor;
        private readonly Func<IActorContext> getContext;
        private List<Tuple<IActorRef, ICommand, Action<IEvent, Action<IEvent>>>> stash = new List<Tuple<IActorRef, ICommand, Action<IEvent, Action<IEvent>>>>();

        public virtual void SetInSync(bool inSync)
        {
            this.inSync = inSync;
            foreach (var cmd in stash)
            {
                OnCommand(cmd.Item1, cmd.Item2, cmd.Item3);
            }
            stash.Clear();
        }

        public virtual void OnCommand(IActorRef sender, ICommand message, Action<IEvent, Action<IEvent>> persist)
        {
            if (!inSync)
            {
                stash.Add(new Tuple<IActorRef, ICommand, Action<IEvent, Action<IEvent>>>(sender, message, persist));
                return;
            }

            switch (message)
            {
                case NotifyUserEvent cmd:
                    persist(cmd.Evt, e => OnEvent(sender, e));
                    break;
                case RequestClaims<TKey> req:
                    ForwardUserRequests(req.UserId, req);
                    break;
                case RequestTokens<TKey> req:
                    ForwardUserRequests(req.UserId, req);
                    break;
                case RequestUserLoginInfo<TKey> req:
                    ForwardUserRequests(req.UserId, req);
                    break;
                case FindById fbi:
                    FindById(fbi.UserId, sender);
                    break;
                case FindByEmail fbe:
                    FindByEmail(fbe.NormalizedEmail, sender);
                    break;
                case FindByClaim fbc:
                    FindByClaim(fbc.Type, fbc.Value, sender);
                    break;
                case FindByLogin fbl:
                    FindByLogin(fbl.ExternalLogin, sender);
                    break;
                case FindByUsername fbu:
                    FindByUsername(fbu.NormalizedUsername, sender);
                    break;
                case CreateUser<TKey, TUser> cu:
                    break;
                case DeleteUser<TKey> du:
                    break;
                case UpdateUser<TKey, TUser> uu:
                    //check important field; logins email username etc.
                    break;
            }
        }

        private void FindByUsername(string normalizedUsername, IActorRef sender)
        {
            if (existingUserNames.TryGetValue(normalizedUsername, out TKey key))
            {
                if (!userActors.TryGetValue(key, out IActorRef actor))
                {
                    userActors[key] = actor = createUserActor(key);
                }
                actor.Tell(ReturnDetails.Instance, sender);
            }
            sender.Tell(NilMessage.Instance);
        }

        private void FindByLogin(ExternalLogin external, IActorRef sender)
        {
            if (existingLogins.TryGetValue(external, out TKey key))
            {
                if (!userActors.TryGetValue(key, out IActorRef actor))
                {
                    userActors[key] = actor = createUserActor(key);
                }
                actor.Tell(ReturnDetails.Instance, sender);
            }
            sender.Tell(NilMessage.Instance);
        }

        private void FindById(string userId, IActorRef sender)
        {
            if (existingIds.TryGetValue(userId, out TKey key))
            {
                if (!userActors.TryGetValue(key, out IActorRef actor))
                {
                    userActors[key] = actor = createUserActor(key);
                }
                actor.Tell(ReturnDetails.Instance, sender);
            }
            sender.Tell(NilMessage.Instance);
        }

        private void FindByEmail(string email, IActorRef sender)
        {
            if (existingEmails.TryGetValue(email, out TKey key))
            {
                if (!userActors.TryGetValue(key, out IActorRef actor))
                {
                    userActors[key] = actor = createUserActor(key);
                }
                actor.Tell(ReturnDetails.Instance, sender);
            }
            sender.Tell(NilMessage.Instance);
        }

        private void FindByClaim(string type, string value, IActorRef sender)
        {
            //usersByClaims
            if (usersByClaims.TryGetValue(type, out Dictionary<string, HashSet<TKey>> byType)
                && byType.TryGetValue(value, out HashSet<TKey> users))
            {
                var actors = new List<IActorRef>();

                foreach (var key in users)
                {
                    if (!userActors.TryGetValue(key, out IActorRef actor))
                    {
                        userActors[key] = actor = createUserActor(key);
                    }

                    actors.Add(actor);
                    //actor.Tell(ReturnDetails.Instance, sender);
                }
                if (actors.Count > 0)
                {
                    var context = getContext();
                    var aggregator = context.ActorOf(Props.Create(() => new Aggregator<TUser>(actors)));
                    aggregator.Ask<AggregatedReply<TUser>>(ReturnDetails.Instance).PipeTo(sender);
                    return;
                }
            }
            sender.Tell(new TUser[0]);
        }

        private void ForwardUserRequests(TKey userId, IGetUserProperties req)
        {
            if (!userActors.TryGetValue(userId, out IActorRef actor))
            {
                userActors[userId] = actor = createUserActor(userId);
            }
            actor.Forward(req);
        }

        /// <summary>
        /// These events are raised by the user actor, or by the persistence layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public virtual void OnEvent(IActorRef sender, IEvent message)
        {
            switch (message)
            {
                //when events are not meant for the coordinator, but are persisted in the coordinator
                case IUserEvent<TKey> evt:
                    break;
                case UserCreated<TKey> evt:
                    break;
                case UserDeleted<TKey> evt:
                    break;
                case UserNameChanged<TKey> evt:
                    break;
                case UserLoginAdded<TKey> evt:
                    break;
                case UserLoginRemoved<TKey> evt:
                    break;
                case UserEmailChanged<TKey> evt:
                    break;
                case UserClaimsAdded<TKey> evt:
                    break;
                case UserClaimsRemoved<TKey> evt:
                    break;
            }
        }
    }
}
