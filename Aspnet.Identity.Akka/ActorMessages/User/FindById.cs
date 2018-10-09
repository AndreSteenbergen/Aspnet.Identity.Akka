using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindById : ICommand
    {
        public FindById(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; }
    }
}