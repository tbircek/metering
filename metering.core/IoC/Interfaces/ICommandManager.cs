using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles saving and loading the test step(s)
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// saves the test step to the user specified location.
        /// </summary>
        /// <returns></returns>
        Task SaveNewTestAsync();

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
