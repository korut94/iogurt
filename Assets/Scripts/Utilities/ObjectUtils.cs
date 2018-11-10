using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Iogurt.Utilities
{
    public static class ObjectUtils
    {
        public static IEnumerable<Type> GetImplementationsOfInterface(Type type)
        {
            if (type.IsInterface)
                return GetAssignableTypes(type);

            return Enumerable.Empty<Type>();
        }

        public static IEnumerable<Type> GetExtensionsOfClass(Type type)
        {
            if (type.IsClass)
                return GetAssignableTypes(type);

            return Enumerable.Empty<Type>();
        }

        static IEnumerable<Type> GetAssignableTypes(Type type, Func<Type, bool> predicate = null)
        {
            var list = new List<Type>();
            ForEachType(t =>
            {
                if (type.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract && (predicate == null || predicate(t)))
                    list.Add(t);
            });

            return list;
        }

        static void ForEachAssembly(Action<Assembly> callback)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try 
                {
                    callback(assembly);
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip any assemblies that don't load properly -- suppress errors
                }
            }
        }

        static void ForEachType(Action<Type> callback)
        {
            ForEachAssembly(assembly =>
        {
                var types = assembly.GetTypes();
                foreach (var t in types)
                    callback(t);
            });
        }
    }
}

