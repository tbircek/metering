
namespace metering.core
{
    /// <summary>
    /// a logger that will handle log messages from a <see cref="ILogFactory"/>
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// handles the logged message being passed in
        /// </summary>
        /// <param name="message">the message being log</param>
        /// <param name="level">the level of the log message</param>
        void Log(string message, LogLevel level);

    }
}
