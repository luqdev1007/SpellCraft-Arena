using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Infrastructure.DI
{
    public class DIContainer
    {
        private readonly Dictionary<Type, object> _container = new();

        public void RegisterAsSingle<T>(Func<DIContainer, T> creator)
        {
            Registration registration = new Registration(container => creator.Invoke(container));
            _container.Add(typeof(T), registration);
        }
    }

    public class Registration
    {
        private Func<DIContainer, object> _creator;
        private object _cachedInstance;

        public Registration(Func<DIContainer, object> creator) => _creator = creator;

        public object CreateInstanceFrom(DIContainer container)
        {
            if (_cachedInstance != null)
                return _cachedInstance;

            if (_creator == null)
                throw new InvalidOperationException("No has instance or creator");

            _cachedInstance = _creator.Invoke(container);

            return _cachedInstance;
        }
    }
}
