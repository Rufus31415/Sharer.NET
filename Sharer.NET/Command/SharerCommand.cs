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
        ReadVariable,
        WriteVariable,
        SubscribeVariable,
        UnsubscribeVariable,
    }
}
