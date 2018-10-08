namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindById
    {
        public FindById(string userId)
        {
            UserId = userId;
        }
        public string UserId { get; }
    }
}