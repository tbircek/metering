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

        ///// <summary>
        ///// holder for the mode
        ///// </summary>
        //private string mode;

        /// <summary>
        /// holder for the WiringDiagramFileLocation
        /// </summary>
        private string wiringDiagramFileLocation;

        #endregion

        #region Public Properties

        /// <remarks>For more information 
        /// "CMEngine.pdf" page 206 of Test Universe 4.00 CMEngine.ENU.21 — Year: 2018</remarks>
        /// 
        /// <summary>
        /// ID of the configuration to be used in the amp:route, if you
        /// want to route a triple to this configuration.
        /// This is a number of type integer. 
        /// However to combined amplifiers this app will hold it as string.
        /// </summary>
        public string ConfigID { get; set; } = string.Empty;

        /// <summary>
        /// Number of phases of the virtual amplifier. It is the number of phases that can be
        /// independently addressed and set with the out:ana commands.The number of
        /// phases that are actually output may be different (for instance, when Vo is
        /// automatically calculated from the three phase voltages in the CMC 256 or newer
        /// test set). This is a number of type integer.
        /// </summary>
        public int PhaseCount { get; set; } = 0;

        /// <summary>
        /// a detailed text reference to Omicron Hardware Configuration available to the specific
        /// Omicron Test Set. ex: "3x300V, 85VA @ 85V, 1Arms"
        /// </summary>
        public string WiringDiagramString { get; set; } = string.Empty;

        /// <summary>
        /// path to the wiring diagram associated with this hardware configuration
        /// </summary>
        public string WiringDiagramFileLocation
        {
            get
            {
                // return wiring diagram location and file name.
                return $"../Images/Omicron/{wiringDiagramFileLocation}.png";
            }
            set
            {
                // if new selection is different than previous
                if (!Equals(value, wiringDiagramFileLocation))
                {
                    // update the old value.
                    wiringDiagramFileLocation = value;
                }
            }
        }

        /// <summary>
        /// It encodes the way the different outputs of the physical
        /// amplifiers involved in the configuration are to be tied together to achieve the
        /// configuration’s characteristics. This value would decide which diagram to show to the user.
        /// Value of type string. 
        /// </summary>
        public string Mode { get; set; } = string.Empty;
        //{
        //    get
        //    {
        //        // return wiring diagram location and file name.
        //        return $"../Images/Omicron/{mode}.png";
        //    }
        //    set
        //    {
        //        // if new selection is different than previous
        //        if (!Equals(value, mode))
        //        {
        //            // update the old value.
        //            mode = value;
        //        }
        //    }
        //}

        /// <summary>
        /// Holds radio button group names
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Holds whether check box selected or not
        /// </summary>
        public bool CurrentWiringDiagram { get; set; }

        /// <summary>
        /// Internal OMICRON identifier. It refers to a connection diagram depicting the
        /// connections encoded in <mode>. This field is of integer type.
        /// </summary>
        public int WiringID { get; set; } = -1;

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
