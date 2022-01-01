using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AssemblyChecker;
using BlockInfo = AssemblyChecker.BlockInfo;



namespace AssemblyChecker
{
    class AssemblyCheck
    {
        public List<Container> GetAssemblyInfo(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (!extension.Equals(".dll") && !extension.Equals(".exe"))
            {
                throw new ExeptionsChecker("Passed filepath is not assembly");
            }

            var assembly = Assembly.LoadFrom(filePath);

            var assemblyInfo = new Dictionary<string, Container>();

            foreach (var type in assembly.GetTypes())
            {
                try
                {
                    if (!assemblyInfo.ContainsKey(type.Namespace))
                        assemblyInfo.Add(type.Namespace, new Container(type.Namespace, ClassFormatter.Format(type)));

                    assemblyInfo.TryGetValue(type.Namespace, out var container);

                    container.Members.Add(GetMembers(type));

                    if (type.IsDefined(typeof(ExtensionAttribute), false))
                        assemblyInfo = GetExtensionNamespaces(type, assemblyInfo);

                }
                catch (NullReferenceException e) { Console.WriteLine(e.StackTrace); }
            }

            return assemblyInfo.Values.ToList();
        }

        private static Dictionary<string, Container> GetExtensionNamespaces(Type classType, Dictionary<string, Container> assemblyInfo)
        {
            var extensionClasses = new Dictionary<string, Container>();

            foreach (var method in classType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!classType.IsDefined(typeof(ExtensionAttribute), false) ||
                    !method.IsDefined(typeof(ExtensionAttribute), false)) continue;

                var type = method.GetParameters()[0].ParameterType;

                if (!assemblyInfo.ContainsKey(type.Namespace))
                    assemblyInfo.Add(type.Namespace, new Container(type.Namespace, ClassFormatter.Format(type)));

                Container @class = new Container(ClassFormatter.Format(type), ClassFormatter.Format(type));
                @class.Members.Add(new BlockInfo(MethodFormatter.Format(method) + " — метод расширения", ClassFormatter.Format(classType)));

                assemblyInfo.TryGetValue(type.Namespace, out var container);
                container.Members.Add(@class);

            }

            return assemblyInfo;
        }

        private static Container GetMembers(Type type)
        {
            var member = new Container(ClassFormatter.Format(type), ClassFormatter.Format(type));

            var members = GetFields(type);
            members.AddRange(GetProperties(type));
            members.AddRange(GetMethods(type));

            member.Members = members;

            return member;
        }

        private static IEnumerable<BlockInfo> GetMethods(Type type)
        {
            var methodInfos = new List<BlockInfo>();

            // add constructors
            methodInfos.AddRange(GetConstructors(type));

            // add methods
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {

                if (type.IsDefined(typeof(ExtensionAttribute), false) && method.IsDefined(typeof(ExtensionAttribute), false))
                    continue;

                var signature = MethodFormatter.Format(method);
                methodInfos.Add(new BlockInfo(signature, ClassFormatter.Format(type)));
            }

            return methodInfos;
        }

        private static IEnumerable<BlockInfo> GetConstructors(Type type)
        {
            return type.GetConstructors().Select(constructor => new BlockInfo(ConstructorFormatter.Format(constructor), ClassFormatter.Format(type))).ToArray();
        }

        private static List<BlockInfo> GetFields(Type type)
        {
            return type.GetFields().Select(field => new BlockInfo(FieldFormatter.Format(field), ClassFormatter.Format(type))).ToList(); //Instance | Static | Public | NonPublic
        }

        private static IEnumerable<BlockInfo> GetProperties(Type type)
        {
            return type.GetProperties().Select(property => new BlockInfo(PropertyFormatter.Format(property), ClassFormatter.Format(type))).ToList();
        }
    }
}
