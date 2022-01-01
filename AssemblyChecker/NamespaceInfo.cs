using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyChecker
{
    class NamespaceInfo
    {
        public string Signature { get; set; }

        public List<ContainerInAssembly> MemberInfo { get; set; }

        public NamespaceInfo(string signature)
        {
            MemberInfo = new List<ContainerInAssembly>();
            Signature = signature;
        }

        public NamespaceInfo(List<ContainerInAssembly> memberInfo, string signature)
        {
            Signature = signature;
            MemberInfo = memberInfo;
        }
    }
}
