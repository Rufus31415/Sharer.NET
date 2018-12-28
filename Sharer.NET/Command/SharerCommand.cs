using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.Command
{
    public enum SharerCommandID : byte
    {
        None = 0x00,
        FunctionCount,
        FunctionPrototype,
        AllFunctionsPrototype,
        CallFunction,
        AllVariablesDefinition,
        ReadVariables,
        WriteVariables,

        NotificationCommand = 0x80,
        Error = 0x80,
        Ready
    }
}
