using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="SettingsListViewModel"/> Voltage configurations.
    /// </summary>
    public class SettingsDesignModelVoltage : SettingsViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static SettingsDesignModelVoltage Instance => new SettingsDesignModelVoltage();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public SettingsDesignModelVoltage()
        {
            OmicronVoltageOutputs = new ObservableCollection<SettingsListItemViewModel>
            {
                 new SettingsListItemViewModel
                {
                    // This is not in Omicron returns: 0,9,3,3.000000e+02,8.500000e+01,8.500000e+01,1.000000e+00,std,0,amp_no,1;
                    // Omicron UI shows: 4x300V, 85VA @ 85V, 1Arms
                    ConfigID = "9", // 9,
                    WiringDiagramString = "4x300V, 85VA @ 85V, 1Arms",
                    Mode = "std", // std,0,
                },

                new SettingsListItemViewModel
                {
                    // Omicron returns: 0,9,3,3.000000e+02,8.500000e+01,8.500000e+01,1.000000e+00,std,0,amp_no,1;
                    // Omicron UI shows: 3x300V, 85VA @ 85V, 1Arms
                    ConfigID = "9", // 9,
                    WiringDiagramString = "3x300V, 85VA @ 85V, 1Arms",
                    Mode = "std", // std,0,
                },

                new SettingsListItemViewModel
                {
                    // 0,10,1,3.000000e+02,1.500000e+02,7.500000e+01,2.000000e+00,std,14,amp_no,5;
                    // Omicron UI shows: 1x300V, 150VA @ 75V, 2Arms
                    ConfigID = "10", // 10,
                    WiringDiagramString = "1x300V, 150VA @ 75V, 2Arms",
                    Mode = "std", // std,14,    
                },

                new SettingsListItemViewModel
                {
                    // 0,11,3,3.000000e+02,5.000000e+01,7.500000e+01,6.600000e-01,zero,13,amp_no,1,amp_no,5;
                    // Omicron UI shows: 3x300V, 50VA @ 75V, 660mArms
                    ConfigID = "11", // 11,
                    WiringDiagramString = "3x300V, 50VA @ 75V, 660mArms, VE automatically calculated",
                    Mode = "zero", // zero, 
                },

                new SettingsListItemViewModel
                {
                    // 0,12,1,6.000000e+02,2.500000e+02,2.000000e+02,1.250000e+00,ser13,4,amp_no,1;
                    // Omicron UI shows: 1x600V, 250VA @ 200V, 1.25Arms
                    ConfigID = "12", // 12,                    
                    WiringDiagramString = "1x600V, 250VA @ 200V, 1.25Arms",
                    Mode = "ser13", // ser13,4, 
                },

                new SettingsListItemViewModel
                {
                    // 0,13,2,6.000000e+02,1.250000e+02,1.500000e+02,1.000000e+00,ser2,17,amp_no,1,amp_no,5;
                    // Omicron UI shows: 2x600V, 125VA @ 150V, 1Arms
                    ConfigID = "13", // 13,
                    WiringDiagramString = "2x600V, 125VA @ 150V, 1Arms",
                    Mode = "ser2", // ser2,17,  
                },
            };
        }

        #endregion
    }
}
