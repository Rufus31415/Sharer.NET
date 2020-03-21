using Sharer.FunctionCall;
using Sharer.Variables;
using System.Collections.Generic;

namespace Sharer.Command
{
    /// <summary>
    /// Command that allows to encode and decode the list of all variables shared
    /// </summary>
    internal class SharerGetAllVariablesDefinitionCommand : SharerSentCommand
    {
        internal override SharerCommandID CommandID => SharerCommandID.AllVariablesDefinition;

        internal override byte[] ArgumentsToSend()
        {
            return null;
        }

        /// <summary>
        /// internal state machine for decoding the list
        /// </summary>
        private enum Steps
        {
            VariableCountHigh,
            VariableCountLow,
            VariableType,
            VariableName,
            End
        }

        // reception step
        private Steps _receivedStep = Steps.VariableCountHigh;

        // number of expected functions
        private int _nbVariables = 0;

        public List<SharerVariable> Variables = new List<SharerVariable>();

        private SharerVariable _currentVariable;

        private List<byte> _nameByte = new List<byte>(20);

        internal override bool DecodeArgument(byte b)
        {
            switch (_receivedStep)
            {
                case Steps.VariableCountHigh: // get number of functions
                    Variables.Clear();
                    _nbVariables = b;
                    _receivedStep = Steps.VariableCountLow;
                    break;
                case Steps.VariableCountLow:
                    _nbVariables = _nbVariables + 256 * b;
                    if (_nbVariables == 0)
                    {
                        _receivedStep = Steps.End; // if no function, stop parsing
                    }
                    else
                    {
                        _receivedStep = Steps.VariableType;
                    }
                    break;
                case Steps.VariableType: // get return type
                    _currentVariable = new SharerVariable();
                    _currentVariable.Type = (SharerType)b;
                    _nameByte.Clear();
                    _receivedStep = Steps.VariableName;
                    break;
                case Steps.VariableName: // get function name
                    // if end of name
                    if (b == 0)
                    {
                        _currentVariable.Name = System.Text.Encoding.UTF8.GetString(_nameByte.ToArray(), 0, _nameByte.Count);
                        Variables.Add(_currentVariable);

                        if (Variables.Count >= _nbVariables) // if enought function
                            {
                                _receivedStep = Steps.End;
                            }
                            else // else, go to next function
                            {
                                _receivedStep = Steps.VariableType;
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
