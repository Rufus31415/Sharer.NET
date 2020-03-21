using System;
using System.Threading;

namespace Sharer.Command
{
    /// <summary>
    /// Abstract class to describe a Sharer command, encode and decode it
    /// </summary>
    public abstract class SharerSentCommand
    {
        internal abstract SharerCommandID CommandID { get; }

        internal abstract byte[] ArgumentsToSend();

        internal event EventHandler Timeouted;

        internal byte SentID;

        // return True when decodage is done
        internal abstract bool DecodeArgument(byte b);


        internal void BeginSend(byte id)
        {
            SentID = id;
            _autoResetEvent.Reset();
        }

        AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private bool _AnswerReceived;

        /// <summary>
        /// Internal exception thrown during command execution
        /// </summary>
        public Exception Exception { get; private set; }

        internal void EndReceive(bool success, Exception ex = null)
        {
            this._AnswerReceived = success;
            this.Exception = ex;

            _autoResetEvent.Set();
        }

        /// <summary>
        /// Wait command to finish call and answer
        /// </summary>
        /// <param name="timeout">Maximum blocking time</param>
        /// <returns>True if success</returns>
        public bool WaitAnswer(TimeSpan timeout)
        {
          bool success = _autoResetEvent.WaitOne(timeout);

          if (!success) Timeouted?.Invoke(this, EventArgs.Empty);

          return success && _AnswerReceived;
        }
    }
}
