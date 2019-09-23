
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles reading/writing and querying the file system
    /// </summary>
    public class FileManager : IFileManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileManager()
        {

        }

        /// <summary>
        /// writes the text to the specified file
        /// </summary>
        /// <param name="text">the text to write</param>
        /// <param name="path">the path of the file</param>
        /// <param name="append">if true, writes the text to the end of the file, otherwise overrides any existing file</param>
        /// <returns></returns>
        public async Task WriteTextToFileAsync(string text, string path, bool append = false)
        {
            // TODO: Add exception catching

            // normalize and resolve path
            path = NormalizePath(path);

            // resolve to absolute path
            path = ResolvePath(path);

            // lock the task
            await AsyncAwaiter.AwaitAsync(nameof(FileManager) + path, async () =>
            {                
                // run the synchronous file access as new task
                await IoC.Task.Run(() =>
                {
                    // write the log message to a file
                    using (var fileStream = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                        fileStream.Write(text);

                });
            });
        }


        /// <summary>
        /// normalizes a path based on the current operating system
        /// </summary>
        /// <param name="path">the path to normalize</param>
        /// <returns></returns>
        public string NormalizePath(string path)
        {
            // if on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                // replace any / with \\ 
                return path?.Replace('/', '\\').Trim();
            // if on Linux/Mac
            else
                // replace any \\ with /
                return path?.Replace('\\', '/').Trim();

        }


        /// <summary>
        /// resolves any relative elements of the path to absolute
        /// </summary>
        /// <param name="path">the path to resolve</param>
        /// <returns></returns>
        public string ResolvePath(string path)
        {
            // resolve the path
            return Path.GetFullPath(path);
        }

        #endregion
    }
}
