using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Sharer.FunctionCall
{
    /// <summary>
    /// Description of a function shared with Sharer
    /// </summary>
    public class SharerFunction
    {
        /// <summary>
        /// Function name
        /// </summary>
        public string Name;

        /// <summary>
        /// Arguments of the function
        /// </summary>
        public readonly List<SharerFunctionArgument> Arguments = new List<SharerFunctionArgument>();

        /// <summary>
        /// Return type of the function
        /// </summary>
        public SharerType ReturnType = SharerType.@void;

        /// <summary>
        /// Human readable string
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Get the prototype of the function. For example: int myFunction(int arg1, bool arg2)
        /// </summary>
        public string Prototype
        {
            get
            {
                var sb = new StringBuilder(ReturnType.ToString());
                sb.Append(" ");
                sb.Append(Name);
                sb.Append("(");
                sb.Append(string.Join(", ", Arguments.Select((x) => $"{x.Type.ToString()} {x.Name}").ToArray()));
                sb.Append(")");
                return sb.ToString();
            }
        }
    }
}
