using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// a view model for each analog signal in the SettingsPage
    /// </summary>
    public class SettingsListItemViewModel : BaseViewModel
    {
        #region Private Properties

        /// <summary>
        /// holder for the mode
        /// </summary>
        private string mode;
        #endregion

        #region Public Properties

        /// <remarks>For more information 
        /// "CMEngine.pdf" page 206 of Test Universe 4.00 CMEngine.ENU.21 — Year: 2018</remarks>
        /// 
        /// <summary>
        /// ID of the configuration to be used in the amp:route, if you
        /// want to route a triple to this configuration.
        /// This is a number of type integer. 
        /// </summary>
        public int ConfigID { get; set; } = 0;

        /// <summary>
        /// a detailed text reference to Omicron Hardware Configuration available to the specific
        /// Omicron Test Set. ex: "3x300V, 85VA @ 85V, 1Arms"
        /// </summary>
        public string WiringDiagramString { get; set; } = string.Empty;

        /// <summary>
        /// It encodes the way the different outputs of the physical
        /// amplifiers involved in the configuration are to be tied together to achieve the
        /// configuration’s characteristics. This value would decide which diagram to show to the user.
        /// Value of type string. 
        /// </summary>
        public string Mode
        {
            get
            {
                // return wiring diagram location and file name.
                return $"../Images/Omicron/{mode}.png";
            }
            set
            {
                // if new selection is different than previous
                if (!Equals(value, mode))
                {
                    // update the old value.
                    mode = value;
                }
            }
        }

        /// <summary>
        /// Holds radio button group names
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Holds whether check box selected or not
        /// </summary>
        public bool CurrentWiringDiagram { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the user selected wiring diagram image file location
        /// </summary>
        public ICommand GetWiringDiagramCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public SettingsListItemViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // retrieve wiring diagram image file location and show it to the user.
            GetWiringDiagramCommand = new RelayParameterizedCommand(async (parameter) => await IoC.Settings.GetWiringDiagram(parameter));
        }

        #endregion

        #region Public Method

        #endregion
    }
}
