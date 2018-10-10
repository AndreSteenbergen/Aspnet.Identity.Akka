using System;
using System.Collections.Generic;
using Aspnet.Identity.Akka.Interfaces;

namespace Profile
{
    class SimplePersister
    {
        public List<IEvent> CoordinatorPersistCalled { get; } = new List<IEvent>();
        public List<KeyValuePair<Guid, IEvent>> UserPersistCalled { get; } = new List<KeyValuePair<Guid, IEvent>>();

        public Action<IEvent, Action<IEvent>> CoordinatorPerist { get; }
        public Action<Guid, IEvent, Action<IEvent>> UserPersist { get; }

        public SimplePersister()
        {
            CoordinatorPerist = (e, a) => {
                CoordinatorPersistCalled.Add(e);
                a(e);
            };

            UserPersist = (id, e, a) =>
            {
                UserPersistCalled.Add(new KeyValuePair<Guid, IEvent>(id, e));
                a(e);
            };
        }
    }
}
