﻿using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="SettingsListViewModel"/>
    /// </summary>
    public class SettingsListItemDesignModel : SettingsListItemViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static SettingsListItemDesignModel Instance => new SettingsListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public SettingsListItemDesignModel()
        {
            // Omicron returns: 0,9,3,3.000000e+02,8.500000e+01,8.500000e+01,1.000000e+00,std,0,amp_no,1;
            // Omicron UI shows: 3x300V, 85VA @ 85V, 1Arms
            ConfigIDs = new ObservableCollection<int> { 9 }; // 9,
            PhaseCounts = new ObservableCollection<int> { 4 }; // 4;
            WiringDiagramString = "4x300V, 85VA @ 85V, 1Arms";
            Mode = "std"; // std + 0,
            GroupName = "V";
            CurrentWiringDiagram = true;
        }

        #endregion
    }
}
