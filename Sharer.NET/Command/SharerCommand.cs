namespace Sharer.Command
{
    /// <summary>
    /// List of all commands available in Sharer
    /// </summary>
    internal enum SharerCommandID : byte
    {
        None = 0x00,
        FunctionCount,
        FunctionPrototype,
        AllFunctionsPrototype,
        CallFunction,
        AllVariablesDefinition,
        ReadVariables,
        WriteVariables,
        GetInfo,

        NotificationCommand = 0x80,
        Error = 0x80,
    }
}
