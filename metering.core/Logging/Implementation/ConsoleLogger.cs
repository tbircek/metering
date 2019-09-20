using System;

namespace metering.core
{
    /// <summary>
    /// logs messages to the console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        
        /// <summary>
        /// logs the given message to the console
        /// </summary>       
        /// <param name="message">the message to log</param>
        /// <param name="level">the level of the message</param>
        public void Log(string message, LogLevel level)
        {
            // save old color
            var consoleOldColor = Console.ForegroundColor;

            // default log color
            var consoleColor = ConsoleColor.White;

            // console color based on level
            switch (level)
            {
                // debug is blue
                case LogLevel.Debug:
                    consoleColor = ConsoleColor.Blue;
                    break;
                // verbose is gray
                case LogLevel.Verbose:
                    consoleColor = ConsoleColor.Gray;
                    break;
                // warning is yellow
                case LogLevel.Warning:
                    consoleColor = ConsoleColor.DarkYellow;
                    break;               
                // error is red
                case LogLevel.Error:
                    consoleColor = ConsoleColor.Red;
                    break;
                // success is green
                case LogLevel.Success:
                    consoleColor = ConsoleColor.Green;
                    break;
                default:
                     consoleColor = ConsoleColor.White;
                    break;
            }

            // set desired console color
            Console.ForegroundColor = consoleColor;

            // write message to console
            Console.WriteLine(message);

            // reset color
            Console.ForegroundColor = consoleOldColor;
        }
    }
}
