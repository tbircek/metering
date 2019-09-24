using System;
using System.Diagnostics;

namespace metering.core
{
    /// <summary>
    /// logs messages to the debug window
    /// Native debug window doesn't have color 
    /// so install VS Extension VSColorOutput
    /// </summary>
    public class DebugLogger : ILogger
    {
        
        /// <summary>
        /// logs the given message to the debug window
        /// </summary>       
        /// <param name="message">the message to log</param>
        /// <param name="level">the level of the message</param>
        public void Log(string message, LogLevel level = LogLevel.Debug)
        {
            // default category
            var category = default(string);
                       
            // color based on level
            switch (level)
            {
                // debug
                case LogLevel.Debug:
                    category = "debug";
                    break;
                // verbose
                case LogLevel.Verbose:
                    category = "verbose";
                    break;
                // informative
                case LogLevel.Informative:
                    category = "informative";
                    break;
                // warning
                case LogLevel.Warning:
                    category = "warning";
                    break;               
                // error
                case LogLevel.Error:
                    category = "error";
                    break;
                // success
                case LogLevel.Success:
                    category = "-----";
                    break;
                default:
                    category = "debug";
                    break;
            }

            // Write message to debug window
            Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: {message}", category);
        }
    }
}
