using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyChecker
{
    public class BlockInfo
    {
        public string Signature { get; set; }
        public string Class { get; set; }

        public BlockInfo(string signature, string @class)
        {
            Signature = signature;
            Class = @class;
        }
    }
}
