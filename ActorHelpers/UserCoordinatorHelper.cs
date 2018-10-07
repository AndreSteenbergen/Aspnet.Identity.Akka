using Akka.Actor;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.ActorMessages.UserCoordinator;
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
        public UserCoordinatorHelper(Func<TKey, IActorRef> createUserActor, Action<IActorRef> killUserActor)
        {
            this.createUserActor = createUserActor;
            this.killUserActor = killUserActor;
        }

        private bool inSync;

        //useractors, as created by the real user coordinators
        private Dictionary<TKey, IActorRef> userActors = new Dictionary<TKey, IActorRef>();

        //just to find users by key, username, email and external login.
        private HashSet<TKey> allUserIds = new HashSet<TKey>();
        private Dictionary<TKey, Tuple<string, string, HashSet<ExternalLogin>>> existingUsers = new Dictionary<TKey, Tuple<string, string, HashSet<ExternalLogin>>>();
        private Dictionary<string, TKey> existingUserNames = new Dictionary<string, TKey>();
        private Dictionary<string, TKey> existingEmails = new Dictionary<string, TKey>();
        private Dictionary<ExternalLogin, TKey> existingLogins = new Dictionary<ExternalLogin, TKey>();

        private readonly Func<TKey, IActorRef> createUserActor;
        private readonly Action<IActorRef> killUserActor;

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
                case RequestClaims<TKey> req:
                    break;
                case RequestTokens<TKey> req:
                    break;
                case RequestUserLoginInfo<TKey> req:
                    break;
                case FindById fbi:
                    break;
                case FindByEmail fbe:
                    break;
                case FindByClaim fbc:
                    break;
                case FindByLogin fbl:
                    break;
                case FindByUsername fbu:
                    break;
                case CreateUser<TKey, TUser> cu:
                    break;
                case DeleteUser<TKey> du:
                    break;
                case UpdateUser<TKey, TUser> uu:
                    break;
            }
        }

        public virtual void OnEvent(IActorRef sender, IEvent message)
        {
            switch (message)
            {
                //important to keep track of ids, logins, usernames, claims and email
                case UserCreated<TKey> uc:
                    break;
                case UserNameChanged<TKey> unc:
                    break;
                case UserLoginAdded<TKey> ulc:
                    break;
                case UserLoginRemoved<TKey> ulr:
                    break;
                case UserEmailChanged<TKey> umc:
                    break;
                case UserClaimsAdded<TKey> uca:
                    break;
                case UserClaimsRemoved<TKey> ucr:
                    break;
            }
        }

        //private void HandleEvent(IActorRef sender, UserCreated<TKey, TUser> userCreated)
        //{
        //    var usr = userCreated.User;
        //    var hs = new HashSet<ExternalLogin>(usr.Logins.Select(x => new ExternalLogin(x.LoginProvider, x.ProviderKey)).ToArray());

        //    allUserIds.Add(usr.Id);

        //    //just to avoid race conditions
        //    existingUsers.Add(usr.Id, new Tuple<string, string, HashSet<ExternalLogin>>(usr.NormalizedUserName, usr.NormalizedEmail, hs));
        //    existingUserNames.Add(usr.NormalizedUserName, usr.Id);
        //    existingEmails.Add(usr.NormalizedEmail, usr.Id);
        //    foreach (var extLogin in hs)
        //    {
        //        existingLogins.Add(extLogin, usr.Id);
        //    }

        //    userActors[usr.Id] = createUserActor(usr);

        //    if (inSync)
        //    {
        //        sender.Tell(IdentityResult.Success);
        //    }
        //}

        //private void HandleEvent(IActorRef sender, UserDeleted<TKey> userDeleted)
        //{
        //    var tuple = existingUsers[userDeleted.UserId];

        //    //remove indices
        //    existingUserNames.Remove(tuple.Item1);
        //    existingEmails.Remove(tuple.Item2);
        //    foreach (var extLogin in tuple.Item3)
        //    {
        //        existingLogins.Remove(extLogin);
        //    }
        //    existingUsers.Remove(userDeleted.UserId);
        //    killUserActor(userActors[userDeleted.UserId]);
        //    userActors.Remove(userDeleted.UserId);

        //    if (inSync)
        //    {
        //        sender.Tell(IdentityResult.Success);
        //    }
        //}
    }
}
