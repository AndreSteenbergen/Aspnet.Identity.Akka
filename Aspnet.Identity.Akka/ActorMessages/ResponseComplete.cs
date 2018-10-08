namespace Aspnet.Identity.Akka.ActorMessages
{
    class ResponseComplete
    {
        private ResponseComplete() { }
        public static readonly ResponseComplete Instance = new ResponseComplete();
    }
}
