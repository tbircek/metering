using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// Base view model provides command functionality across view models
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Public Properties

        /// <summary>
        /// Application title with version value ignoring revision #
        /// </summary>
        public string AppTitle { get; private set; } = $"{Resources.Strings.Title} (v{Assembly.GetEntryAssembly().GetName().Version.ToString(3)})";

        #endregion

        /// <summary>
        /// the event when any property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// call this to fire <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));

            // these line requires .NET Core 3.0 Preview RC1 as of 9/26/2016
            //if (!DesignerProperties.GetIsInDesignMode(this))
            //IoC.Logger.Log($"(name: {name}) processed successfully.");
            System.Diagnostics.Debug.WriteLine($"(name: {name}) processed successfully.");
        }

        #region Helpers

        #endregion
    }
}
