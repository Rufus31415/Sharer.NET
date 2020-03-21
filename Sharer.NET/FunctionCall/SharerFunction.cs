using System.Collections.Generic;

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
        public List<SharerFunctionArgument> Arguments = new List<SharerFunctionArgument>();

        /// <summary>
        /// Return type of the function
        /// </summary>
        public SharerType ReturnType = SharerType.@void;
    }
}
