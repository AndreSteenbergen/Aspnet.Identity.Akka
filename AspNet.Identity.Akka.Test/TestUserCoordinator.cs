using Akka.Actor;
using Akka.TestKit.Xunit2;
using Aspnet.Identity.Akka.ActorMessages.User;
using Aspnet.Identity.Akka.Actors;
using Microsoft.AspNetCore.Identity;
using System;
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

            var persister = new SimplePersister();

            var userCoordinator = Sys.ActorOf(Props.Create(() => new UserCoordinator<Guid, TestIdentityUser>(
                true,
                persister.CoordinatorPerist,
                persister.UserPersist)));

            userCoordinator.Tell(new CreateUser<Guid, TestIdentityUser>(user));
            var result = ExpectMsg<IdentityResult>().Succeeded;

            Assert.True(result);

            Assert.Single(persister.CoordinatorPersistCalled);
            Assert.Single(persister.UserPersistCalled);
        }
    }
}
