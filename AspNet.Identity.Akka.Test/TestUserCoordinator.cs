using Akka.Actor;
using Akka.TestKit.Xunit2;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.Actors;
using Aspnet.Identity.Akka.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Xunit;

namespace AspNet.Identity.Akka.Test
{
    public class TestUserCoordinator : TestKit
    {
        [Fact]
        public void TestCreateUserAndGettingById()
        {
            var userId = Guid.NewGuid();
            var user = new TestIdentityUser(userId) {
                UserName = "testUser",
                PasswordHash = "secret"
            };

            var coordinatorPersistCalled = new List<IEvent>();
            var userPersistCalled = new List<KeyValuePair<Guid, IEvent>>();
            Action<IEvent, Action<IEvent>> coordinatorPerist = (e, a) => {
                coordinatorPersistCalled.Add(e);
                a(e);
            };
            Action<Guid, IEvent, Action<IEvent>> childPersist = (id, e, a) =>
            {
                userPersistCalled.Add(new KeyValuePair<Guid, IEvent>(id, e));
                a(e);
            };
            
            var userCoordinator = Sys.ActorOf(Props.Create(() => new UserCoordinator<Guid, TestIdentityUser>(
                true,
                coordinatorPerist,
                childPersist)));

            userCoordinator.Tell(new CreateUser<Guid, TestIdentityUser>(user));
            var result = ExpectMsg<IdentityResult>().Succeeded;
            Assert.True(result);
        }
    }
}
