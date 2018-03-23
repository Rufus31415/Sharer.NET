using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.FunctionCall
{
    public class SharerFunction
    {
        public string Name;

        public List<SharerFunctionArgument> Arguments = new List<SharerFunctionArgument>();

        public SharerType ReturnType = SharerType.@void;
    }
}
