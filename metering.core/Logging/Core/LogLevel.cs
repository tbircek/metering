
namespace metering.core
{
    /// <summary>
    /// the severity of the log message
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// developer specific information
        /// </summary>
        Debug = 1,

        /// <summary>
        /// verbose information
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// general information
        /// </summary>
        Informative = 3,

        /// <summary>
        /// a warning
        /// </summary>
        Warning = 4,

        /// <summary>
        /// an error
        /// </summary>
        Error = 5,

        /// <summary>
        /// a success
        /// </summary>
        Success = 6,

    }
}
