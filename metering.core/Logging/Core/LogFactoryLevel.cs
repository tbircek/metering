
namespace metering.core
{
    /// <summary>
    /// the level of details to output for a logger
    /// </summary>
    public enum LogOutputLevel
    {
        /// <summary>
        /// logs everything
        /// </summary>
        Debug = 1,

        /// <summary>
        /// logs everything except debug information
        /// </summary>
        Verbose = 2,

        /// <summary>
        /// logs all informative messages, excluding debugging and verbose information
        /// </summary>
        Informative = 3,

        /// <summary>
        /// logs only critical errors, warnings and success, but no general information
        /// </summary>
        Critical = 4,

        /// <summary>
        /// logs nothing
        /// </summary>
        Nothing = 7,
    }
}
