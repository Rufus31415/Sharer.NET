using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sharer.Command
{
    public abstract class SharerSentCommand
    {
        public abstract SharerCommandID CommandID { get; }

        internal abstract byte[] ArgumentsToSend();

        internal event EventHandler Timeouted;

        public byte SentID;

        public byte ReceiveID;

        // return True when decodage is done
        internal abstract bool DecodeArgument(byte b);


        internal void BeginSend(byte id)
        {
            SentID = id;
            _autoResetEvent.Reset();
        }

        AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private bool _AnswerReceived;

        public Exception Exception { get; private set; }

        internal void EndReceive(bool success, Exception ex = null)
        {
            this._AnswerReceived = success;
            this.Exception = ex;

            _autoResetEvent.Set();
        }

        public bool WaitAnswer(TimeSpan timeout)
        {
          bool success = _autoResetEvent.WaitOne(timeout);

          if (!success) Timeouted?.Invoke(this, EventArgs.Empty);

          return success && _AnswerReceived;
        }
    }
}
