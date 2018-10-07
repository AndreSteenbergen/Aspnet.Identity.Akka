using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class NormalizedEmailChanged : IEvent
    {
        public NormalizedEmailChanged(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}
