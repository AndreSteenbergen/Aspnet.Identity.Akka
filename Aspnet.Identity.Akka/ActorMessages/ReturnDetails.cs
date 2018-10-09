using Aspnet.Identity.Akka.Interfaces;

namespace Aspnet.Identity.Akka.ActorMessages
{
    class ReturnDetails : ICommand
    {
        private ReturnDetails() { }
        public static readonly ReturnDetails Instance = new ReturnDetails();

    }
}
