
using System;

namespace metering.core
{
    /// <summary>
    /// logs to a specific file
    /// </summary>
    public class FileLogger: ILogger
    {
        #region Public Properties

        /// <summary>
        /// the path to write the log file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// if true, logs the time
        /// </summary>
        public bool LogTime { get; set; } = true;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">the path to log file</param>
        /// <param name="logTime">whether log the time or not</param>
        public FileLogger(string filePath, bool logTime)
        {
            // set the file path property
            FilePath = filePath;

        }

        #endregion

        #region Logger Methods

        /// <summary>
        /// logs the information to the log file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level)
        {
            // get current time
            var currentTime = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // prepends time string if desired
            var timeLogString = LogTime ? $"{currentTime}": string.Empty;

            // write the message to the log file
            IoC.File.WriteTextToFileAsync($"[{timeLogString}] {message}" + Environment.NewLine, FilePath, append: true, useParentFolder: true, newFolderName: "logs");
        }

        #endregion
    }
}
