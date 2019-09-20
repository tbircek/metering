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

    }
}
