
using System;
using Newtonsoft.Json;

namespace metering.core
{
    /// <summary>
    /// Saves active <see cref="TestDetailsViewModel"/> to the user specified location.
    /// </summary>
    public class TestStepsLogger: ILogger
    {
        #region Private Properties
        
        #endregion

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
        public TestStepsLogger(string filePath, bool logTime, TestDetailsViewModel test)
        {

            // set the file path property
            FilePath = filePath;

            // set the log time option
            LogTime = logTime;
            
            // generate a serialized JSON string to save 
            string json = JsonConvert.SerializeObject(test, Formatting.Indented);

            // save generated string
            Log(json);
        }
        
        /// <summary>
        /// logs the information to the Beckwith metering test file
        /// </summary>
        /// <param name="message">serialized JSON object to write.</param>
        /// <param name="level">severity of the message</param>
        public void Log(string message, LogLevel level = LogLevel.Verbose)
        {
            // write the message to the log file
            IoC.File.WriteTextToFileAsync($"{message}" + Environment.NewLine, FilePath, append: false);
        }

        #endregion

        #region Helpers

        #endregion
    }
}
