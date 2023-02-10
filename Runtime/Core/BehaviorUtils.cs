using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviorDesigner
{
    public static class BehaviorUtils
    {
        private static List<Assembly> loadedAssemblies;
        private static Dictionary<string, Type> typeLookup = new Dictionary<string, Type>();

        public static Type GetType(string typeName)
        {
            if (loadedAssemblies == null)
            {
                loadedAssemblies = new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
            }

            if (typeLookup.TryGetValue(typeName, out Type type))
            {
                return type;
            }

            foreach (Assembly assembly in loadedAssemblies)
            {
                type = assembly.GetType(typeName);
                if (type != null)
                {
                    typeLookup.Add(typeName, type);
                    return type;
                }
            }

            return null;
        }
    }
}