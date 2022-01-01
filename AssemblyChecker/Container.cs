using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyChecker
{
    public class Container:BlockInfo
    {
        public List<BlockInfo> Members { get; set; }

        public Container(string @namespace, string @class, string signature, List<BlockInfo> members) : base(@namespace, @class)
        {
            Signature = signature;
            Members = members;
        }
        public Container(string @namespace, string @class) : base(@namespace, @class)
        {
            Members = new List<BlockInfo>();
        }
    }
}
