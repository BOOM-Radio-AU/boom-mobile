using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    public interface ILogging
    {
        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="sender">Origin of the error</param>
        /// <param name="exception">Exception (containing error massage text)</param>
        void Error(object sender, Exception exception);

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="sender">Origin of the message</param>
        /// <param name="message">Info massage text</param>
        void Info(object sender, string message);

        /// <summary>
        /// Logs a warining message
        /// </summary>
        /// <param name="sender">Origin of the warning</param>
        /// <param name="message">Warining massage text</param>
        void Warn(object sender, string message);
    }
}
