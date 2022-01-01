using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyChecker
{
    public class ExeptionsChecker : ArgumentException
    {
        public ExeptionsChecker() { }

        public ExeptionsChecker(string message)
            : base(message) { }

        public ExeptionsChecker(string message, System.Exception inner)
            : base(message, inner) { }
    }
}
