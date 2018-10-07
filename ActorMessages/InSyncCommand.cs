namespace Aspnet.Identity.Akka.ActorMessages
{
    public class InSyncCommand
    {
        private InSyncCommand() { }
        public static readonly InSyncCommand Instance = new InSyncCommand();
    }
}
