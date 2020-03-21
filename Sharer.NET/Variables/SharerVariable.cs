using Sharer.FunctionCall;

namespace Sharer.Variables
{
    /// <summary>
    /// Describes a Shared variable that can be read and written by Sharer
    /// </summary>
    public class SharerVariable
    {
        /// <summary>
        /// Variable name
        /// </summary>
        public string Name;

        /// <summary>
        /// Variable type
        /// </summary>
        public SharerType Type;
    }
}
