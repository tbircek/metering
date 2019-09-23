using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles reading/writing and querying the file system
    /// </summary>
    public interface IFileManager
    {

        /// <summary>
        /// writes the text to the specified file
        /// </summary>
        /// <param name="text">the text to write</param>
        /// <param name="path">the path of the file</param>
        /// <param name="append">if true, writes the text to the end of the file, otherwise overrides any existing file</param>
        /// <returns></returns>
        Task WriteTextToFileAsync(string text, string path, bool append = true);

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
