
using System.IO;
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

            // TODO: normalize and resolve path
            // path = Path.GetFullPath(path.Replace("/", "\\").Trim());

            // lock the task
            await AsyncAwaiter.AwaitAsync(nameof(FileManager) + path, async () =>
            {
                // TODO: add IoC.Task.Run that logs to logger on failure

                // run the synchronous file access as new task
                await Task.Run(() =>
                {
                    // write the log message to a file
                    using (var fileStream = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                        fileStream.Write(text);

                });
            });
        }

        #endregion
    }
}
