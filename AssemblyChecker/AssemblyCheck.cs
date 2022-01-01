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
    public class AssemblyCheck
    {
        public List<ContainerInAssembly> GetAssemblyInfo(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if (!extension.Equals(".dll") && !extension.Equals(".exe"))
            {
                
                throw new ExeptionsChecker("Passed filepath is not assembly");
            }
            Console.WriteLine(filePath);
            var assembly = Assembly.LoadFrom(filePath);
            Console.WriteLine("ASS2");
            var assemblyInfo = new Dictionary<string, ContainerInAssembly>();
            Console.WriteLine(assembly);
            foreach (var type in assembly.GetTypes())
            {
                try
                {
                    if (!assemblyInfo.ContainsKey(type.Namespace))
                        assemblyInfo.Add(type.Namespace, new ContainerInAssembly(type.Namespace, ClassFormatter.Format(type)));

                    assemblyInfo.TryGetValue(type.Namespace, out var container);

                    container.Members.Add(GetMembers(type));

                    if (type.IsDefined(typeof(ExtensionAttribute), false))
                        assemblyInfo = GetExtensionNamespaces(type, assemblyInfo);

                }
                catch (NullReferenceException e) { Console.WriteLine(e.StackTrace); }
            }

            return assemblyInfo.Values.ToList();
        }

        private static Dictionary<string, ContainerInAssembly> GetExtensionNamespaces(Type classType, Dictionary<string, ContainerInAssembly> assemblyInfo)
        {
            var extensionClasses = new Dictionary<string, ContainerInAssembly>();

            foreach (var method in classType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!classType.IsDefined(typeof(ExtensionAttribute), false) ||
                    !method.IsDefined(typeof(ExtensionAttribute), false)) continue;

                var type = method.GetParameters()[0].ParameterType;

                if (!assemblyInfo.ContainsKey(type.Namespace))
                    assemblyInfo.Add(type.Namespace, new ContainerInAssembly(type.Namespace, ClassFormatter.Format(type)));

                ContainerInAssembly @class = new ContainerInAssembly(ClassFormatter.Format(type), ClassFormatter.Format(type));
                @class.Members.Add(new BlockInfo(MethodFormatter.Format(method) + " — метод расширения", ClassFormatter.Format(classType)));

                assemblyInfo.TryGetValue(type.Namespace, out var container);
                container.Members.Add(@class);

            }

            return assemblyInfo;
        }

        private static ContainerInAssembly GetMembers(Type type)
        {
            var member = new ContainerInAssembly(ClassFormatter.Format(type), ClassFormatter.Format(type));

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
