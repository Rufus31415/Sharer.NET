using Sharer.FunctionCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.Command
{
    class SharerGetAllFunctionsPrototypeCommand : SharerSentCommand
    {
        public override SharerCommandID CommandID
        {
            get
            {
                return SharerCommandID.AllFunctionsPrototype;
            }
        }

        internal override byte[] ArgumentsToSend()
        {
            return null;
        }

        private enum Steps
        {
            FunctionCountHigh,
            FunctionCountLow,
            ArgumentCount,
            FunctionReturnType,
            FunctionName,
            ArgumentType,
            ArgumentName,
            End
        }

        // reception step
        private Steps _receivedStep = Steps.FunctionCountHigh;

        // number of expected functions
        private int _nbFunctions = 0;

        private int _nbArguments = 0;

        public List<SharerFunction> Functions = new List<SharerFunction>();

        private SharerFunction _currentFunction;
        private SharerFunctionArgument _currentArgument;

        private List<byte> _nameByte = new List<byte>();

        internal override bool DecodeArgument(byte b)
        {
            switch (_receivedStep)
            {
                case Steps.FunctionCountHigh: // get number of functions
                    Functions.Clear();
                    _nbFunctions = b;
                    _receivedStep = Steps.FunctionCountLow;
                    break;
                case Steps.FunctionCountLow:
                    _nbFunctions = _nbFunctions + 256 * b;
                    if (_nbFunctions == 0)
                    {
                        _receivedStep = Steps.End; // if no function, stop parsing
                    }
                    else
                    {
                        _receivedStep = Steps.ArgumentCount;
                    }
                    break;
                case Steps.ArgumentCount: // get number of functions
                     _currentFunction = new SharerFunction();
                   _nbArguments = b;
                    _receivedStep = Steps.FunctionReturnType;
                    break;
                case Steps.FunctionReturnType: // get return type
                    _currentFunction.ReturnType = (SharerType)b;
                    _nameByte.Clear();
                    _receivedStep = Steps.FunctionName;
                    break;
                case Steps.FunctionName: // get function name
                    // if end of name
                    if (b == 0)
                    {
                        _currentFunction.Name = System.Text.Encoding.Default.GetString(_nameByte.ToArray(), 0, _nameByte.Count);
                        if (_nbArguments > 0) // if this function has arguments
                        {
                            _receivedStep = Steps.ArgumentType;
                        }
                        else
                        {
                            Functions.Add(_currentFunction);

                            if (Functions.Count >= _nbFunctions) // if enought function
                            {
                                _receivedStep = Steps.End;
                            }
                            else // else, go to next function
                            {
                                _receivedStep = Steps.ArgumentCount;
                            }
                        }
                    }
                    else // else, store value
                    {
                        _nameByte.Add(b);
                    }
                    break;
                case Steps.ArgumentType: // Get argument type
                    _currentArgument = new SharerFunctionArgument();
                    _currentArgument.Type = (SharerType)b;
                    _nameByte.Clear();
                    _receivedStep = Steps.ArgumentName;
                    break;
                case Steps.ArgumentName: // Get argument name
                    // if end of name
                    if (b == 0)
                    {
                        _currentArgument.Name = System.Text.Encoding.Default.GetString(_nameByte.ToArray(), 0, _nameByte.Count);
                        _currentFunction.Arguments.Add(_currentArgument);


                        if(_currentFunction.Arguments.Count >= _nbArguments) // if all arguments for this function has been read
                        {
                            Functions.Add(_currentFunction);

                            if (Functions.Count >= _nbFunctions) // if enought functions
                            {
                                _receivedStep = Steps.End;
                            }
                            else // else, go to next function
                            {
                                _receivedStep = Steps.ArgumentCount;
                            }
                        }
                        else // else, go to next argument
                        {
                            _receivedStep = Steps.ArgumentType;
                        }
                    }
                    else // else, store value
                    {
                        _nameByte.Add(b);
                    }
                    break;
                default:
                    break;
            }


            return _receivedStep == Steps.End;
        }


    }
}
