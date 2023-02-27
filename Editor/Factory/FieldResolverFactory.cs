using System;
using System.Collections.Generic;
using System.Reflection;

namespace BehaviorDesigner.Editor
{
    public class FieldResolverFactory
    {
        private static List<Type> resolverTypes;

        public IFieldResolver Create(FieldInfo fieldInfo, BehaviorWindow window)
        {
            CollectAllResolvers();
            foreach (Type type in resolverTypes)
            {
                if ((bool) type.InvokeMember("IsAcceptable", BindingFlags.InvokeMethod, null, null, new object[] {fieldInfo}))
                {
                    return (IFieldResolver) Activator.CreateInstance(type, fieldInfo, window);
                }
            }

            return new UnSupportResolver(fieldInfo);
        }

        private void CollectAllResolvers()
        {
            if (resolverTypes != null)
            {
                return;
            }

            resolverTypes = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                    {
                        continue;
                    }

                    if (type.GetInterface(nameof(IFieldResolver)) == null)
                    {
                        continue;
                    }

                    if (type.GetMethod("IsAcceptable") == null)
                    {
                        continue;
                    }

                    resolverTypes.Add(type);
                }
            }
        }
    }
}