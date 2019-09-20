
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace metering.core
{
    /// <summary>
    /// the standard log factory for the application
    /// logs details to the debug by default
    /// </summary>
    public class BaseLogFactory : ILogFactory
    {

        #region Protected Methods

        /// <summary>
        /// the list of loggers in this factory
        /// </summary>
        protected List<ILogger> mLoggers = new List<ILogger>();

        /// <summary>
        /// a lock for the logger list to keep it thread safe
        /// </summary>
        protected object mLoggersLog = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// The level of logging to output
        /// </summary>
        public LogOutputLevel LogOutputLevel { get; set; }

        /// <summary>
        /// If true, includes the origin of where the log message was logged from
        /// such as the class name, line number and file name
        /// </summary>
        public bool IncludeOriginDetails { get; set; } = true;

        #endregion

        #region Public Events

        /// <summary>
        /// Triggered whenever a new log arrived
        /// </summary>
        public event Action<(string Message, LogLevel Level)> NewLog = (details) => { };

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="loggers">the loggers to add to the factory</param>
        public BaseLogFactory(ILogger[] loggers)
        {
            // add a debug logger
            AddLogger(new DebugLogger());

            // add any other loggers passed in
            if (loggers != null)
                foreach (var logger in loggers)
                    AddLogger(logger);
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Adds the specific logger to this factory
        /// </summary>
        /// <param name="logger">the logger</param>
        public void AddLogger(ILogger logger)
        {
            // lock the list so it is thread safe
            lock (mLoggers)
            {
                // the logger is not already in the list
                if (!mLoggers.Contains(logger))
                    // add the logger to the list
                    mLoggers.Add(logger);
            }
        }


        /// <summary>
        /// removes the specific logger to this factory
        /// </summary>
        /// <param name="logger">the logger</param>
        public void RemoveLogger(ILogger logger)
        {
            // lock the list so it is thread safe
            lock (mLoggers)
            {
                // the logger is in the list 
                if (mLoggers.Contains(logger))
                    // remove the logger from the list
                    mLoggers.Remove(logger);
            }
        }

        /// <summary>
        /// logs the specific message to all loggers in this factory
        /// </summary>
        /// <param name="message">the message to log</param>
        /// <param name="level">the level of the message being logged</param>
        /// <param name="origin">the method/function this message was logged in</param>
        /// <param name="filePath">the code filename that this message was log from</param>
        /// <param name="lineNumber">the line of code in the filename this message was logged from</param>
        public void Log(
            string message,
            LogLevel level = LogLevel.Debug,
            [CallerMemberName] string origin = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {

            // if we should not log the message as the level is too low...
            if ((int)level < (int)LogOutputLevel)
                return;

            // if the user wants to know where the log originated from ...
            if (IncludeOriginDetails)
                // [BaseLogFactory.cs > RemoveLogger() > Line 15 message]
                message = $"[{Path.GetFileName(filePath)} > {origin}() > Line {lineNumber}]: {message}";

            // log to all loggers
            mLoggers.ForEach(logger => logger.Log(message, level));

            // inform listener
            NewLog.Invoke((message, level));
        }

        #endregion
    }
}
