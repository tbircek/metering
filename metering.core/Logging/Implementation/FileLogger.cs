
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="filePath">the path to log file</param>
        public FileLogger(string filePath)
        {
            // set the file path property
            FilePath = filePath;

        }

        #endregion

        #region Logger Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public void Log(string message, LogLevel level)
        {
            IoC.File.WriteTextToFileAsync(message + Environment.NewLine, FilePath, append: true);
        }

        #endregion
    }
}
