
using System;
using System.Diagnostics;
using CMEngine;

namespace metering.core
{
    /// <summary>
    /// Scans for Omicron Test Sets
    /// </summary>
    public class FindCMC
    {
        
        #region Public Methods

        /// <summary>
        /// Scans for Omicron CMC's that associated and NOT locked.
        /// </summary>
        public bool Find()
        {
            // initialize CMEngine class
            IoC.CMCControl.CMEngine = new CMEngine.CMEngine();

            // Scan for attached Omicron Test Sets
            IoC.CMCControl.CMEngine.DevScanForNew();

            // generate storage for the attached Omicron Test Sets
            string deviceList = "";

            // initialize extract parameters function
            ExtractParameters extract = new ExtractParameters();

            // get list of Omicron Test Set attached to this computer but it is unlocked.
            deviceList = IoC.CMCControl.CMEngine.DevGetList(ListSelectType.lsUnlockedAssociated);

            // verify at least one device met search criteria
            if (string.IsNullOrWhiteSpace(deviceList))
            {
                // no Omicron Test Set met search criteria and inform the user.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Unable to find any device attach to this computer\n";

                // return negative result.
                return false;
            }

            // log Omicron Test Set debug information.
            IoC.CMCControl.CMEngine.LogNew(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cmc.log");

            // set log level for Omicron Test Set Logging
            IoC.CMCControl.CMEngine.LogSetLevel((short)CMCControl.LogLevels.Level3);

            // inform the developer about search results.
            Debug.WriteLine($"Found device: {deviceList}", "info");

            // inform the developer about errors.
            Debug.WriteLine($"Error text: {IoC.CMCControl.CMEngine.GetExtError()}");

            // extract the device id that matched search criteria 
            IoC.CMCControl.DeviceID = Convert.ToInt32(extract.Parameters(1, deviceList));

            // attempt to attached device that matched search criteria.
            IoC.CMCControl.CMEngine.DevLock(IoC.CMCControl.DeviceID);

            // inform the user about attached device that matched search criteria.
            IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Connecting device: {extract.Parameters(2, deviceList)}\n";

            // Searches for external Omicron amplifiers and returns a list of IDs.
            // Future use.
            // omicron.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_scan);

            // return positive result.
            return true;
        }

        #endregion
    }
}
