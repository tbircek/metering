using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles saving and loading the test step(s)
    /// </summary>
    public interface ICommandManager
    {
        /// <summary>
        /// Shows a <see cref="SaveFileDialog"/> or <see cref="OpenFileDialog"/> per the user selection.
        /// </summary>
        /// <param name="option"><see cref="FileDialogOption"/> to allow the user select save or open test file(s)</param>
        /// <returns></returns>
        Task ShowFileDialogAsync(FileDialogOption option);

        /// <summary>
        /// Loads multiple tests in order
        /// </summary>
        /// <param name="testFileNumber">Test file number in multiple test file location</param>
        /// <returns></returns>
        void LoadTestFile(int testFileNumber);
    }

    /// <summary>
    /// FileDialog options to generate the UI correctly per the user request.
    /// </summary>
    public enum FileDialogOption
    {
        /// <summary>
        /// Represents <see cref="SaveFileDialog"/> 
        /// </summary>
        Save,

        /// <summary>
        /// Represents <see cref="OpenFileDialog"/>
        /// </summary>
        Open,
    }
}
