namespace Aspnet.Identity.Akka.ActorMessages.User
{
    class FindByClaim
    {
        public FindByClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }
    }
}