using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles saving and loading the test step(s)
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// saves the test step from the screen to the user specified location.
        /// </summary>
        /// <returns>the user selected path with name of the file</returns>
        Task<string> SaveNewTestAsync();

        /// <summary>
        /// loads the test steps from the user specified location to the test strip.
        /// </summary>
        /// <returns>the user selected path(s) of the file(s)</returns>
        Task<string> LoadNewTestsAsync();

        /// <summary>
        /// normalizes a path based on the current operating system
        /// </summary>
        /// <param name="path">the path to normalize</param>
        /// <returns></returns>
        string NormalizePath(string path);

        /// <summary>
        /// resolves any relative elements of the path to absolute
        /// </summary>
        /// <param name="path">the path to resolve</param>
        /// <returns></returns>
        string ResolvePath(string path);
    }
}
