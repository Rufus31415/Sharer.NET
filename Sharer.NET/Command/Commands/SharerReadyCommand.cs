using Sharer.FunctionCall;
using Sharer.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.Command
{

    class SharerReadyCommand : SharerSentCommand
    {

        public SharerReadyCommand()
        {
        }

        public override SharerCommandID CommandID
        {
            get
            {
                return SharerCommandID.Ready;
            }
        }

        internal override byte[] ArgumentsToSend()
        {
            return null;
        }

       
        internal override bool DecodeArgument(byte b)
        {
            return true;
        }
    }
}
