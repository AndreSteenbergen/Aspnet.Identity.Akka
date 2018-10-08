namespace Aspnet.Identity.Akka.ActorMessages
{
    class NilMessage
    {
        private NilMessage() { }
        public static readonly NilMessage Instance = new NilMessage();
    }
}
