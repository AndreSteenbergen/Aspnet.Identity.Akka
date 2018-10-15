using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace Aspnet.Identity.Akka.Serialization.MsgPack
{
    public class AspnetIdentityResolver : IFormatterResolver
    {
        static readonly IFormatterResolver[] resolvers = new[] {
            AspnetIdentityEventResolver.Instance,
            StandardResolver.Instance
        };

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            foreach (var resolver in resolvers)
            {
                var result = resolver.GetFormatter<T>();
                if (result != null) return result;
            }
            return null;
        }

        private AspnetIdentityResolver() { }
        public static IFormatterResolver Instance = new AspnetIdentityResolver();
    }

    public class AspnetIdentityEventResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new AspnetIdentityEventResolver();
        private AspnetIdentityEventResolver() { }

        public IMessagePackFormatter<T> GetFormatter<T>() => FormatterCache<T>.Formatter;

        private static class FormatterCache<T>
        {
            public static IMessagePackFormatter<T> Formatter { get; }
            static FormatterCache() => Formatter = (IMessagePackFormatter<T>)AspnetIdentityResolverGetFormatterHelper.GetFormatter(typeof(T));
        }
    }
}
