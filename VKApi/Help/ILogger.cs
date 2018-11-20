using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKApi.Help
{
    public delegate void WriteLogs(string message);
    #region Не реализованная модель своего Логгера. Заменена на Библиотеку NLog
    public interface ILogger
    {
        void LogMessage(string message, TraceLevel traceLevel);
    }

    public class FileLogger : ILogger
    {
        static string DateTimeMessage { get; set; }
        static TraceSwitch TraceSwitchInfo { get; set; }

        public void LogMessage(string message, TraceLevel traceLevel = TraceLevel.Error)
        {
            TraceSwitchInfo = new TraceSwitch("Trace", "Trace level argument swith");
            TraceSwitchInfo.Level = traceLevel;
            DateTimeMessage = DateTime.Now.ToString("dd:HH:mm:ss");

            #region Messeges from TraceLevel
            Trace.WriteLineIf(TraceSwitchInfo.TraceError, $"{DateTimeMessage}: Error - {message}");
            Trace.WriteLineIf(TraceSwitchInfo.TraceWarning, $"{DateTimeMessage}: Warning - {message}");
            Trace.WriteLineIf(TraceSwitchInfo.TraceInfo, $"{DateTimeMessage}: Info - {message}");
            Trace.WriteLineIf(TraceSwitchInfo.TraceVerbose, $"{DateTimeMessage}: Verbose - {message}");
            #endregion
        }
    }
    #endregion
}
