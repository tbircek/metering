
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Gets available associated Omicron Test Set's hardware configurations.
    /// </summary>
    public class HardwareConfigurations
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public HardwareConfigurations()
        {

        }

        #endregion

        #region Public Method

        /// <summary>
        /// Gets every available hardware configuration from attached Omicron test set.
        /// </summary>
        /// <param name="amplifierType">2 types available. "voltage" or "current"</param>
        /// <returns>Returns a <see cref="SettingsListItemViewModel"/></returns>
        public async Task<ObservableCollection<SettingsListItemViewModel>> Get(string amplifierType)
        {
            // initialize extract parameters function
            ExtractParameters extract = new ExtractParameters();

            // storage for available Omicron Hardware Configurations
            ObservableCollection<SettingsListItemViewModel> outputConfigurations = new ObservableCollection<SettingsListItemViewModel>();

            // storage for available Omicron Hardware Configuration to combine to maximize output
            ObservableCollection<SettingsListItemViewModel> outputConfigurationsToCombine = new ObservableCollection<SettingsListItemViewModel>();

            // returns a string that contains the CMC's test set number and the number of available configurations of type<integer>.
            string configurationInformation = await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg)).Result);

            // retrieve the number of available configurations.
            int totalConfiguration = Convert.ToInt16(extract.Parameters(2, configurationInformation).Replace(oldChar: ';', newChar: ' '));

            // delimiters to split Omicron responses
            string[] delimiterStrings = { ";", "," };
            // delimiter to count amplifiers
            string[] ampDelimiter = { ",amp_no,", ",amp_id," };

            // retrieve all available configurations in reverse order to generate combination configurations.
            for (int i = totalConfiguration; i >= 1; i--)
            {
                // Ex: Voltage response
                // 1,11,3,3.000000e+02,5.000000e+01,7.500000e+01,6.600000e-01,zero,13,amp_no,1,amp_no,5;
                // Ex: Current response
                // 1,16,3,1.250000e+01,7.000000e+01,7.500000e+00,1.000000e+01,zero,40,amp_no,2,amp_no,6;

                // storage for available Omicron Hardware Configuration
                SettingsListItemViewModel outputConfiguration = new SettingsListItemViewModel();

                // two options available. either voltage or current.
                string amplifierInitial = string.Empty;
                // if amplifierInitial == "V" than "A", if amplifierInitial == "A" than "V"
                string outputType = string.Empty;
                // record amplifiers' output signal
                string amplifierDescriptor = string.Empty;

                // retrieve Omicron Hardware Configuration Details
                string configurationDetails = (await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg_0, i)).Result));

                // count amplifiers in configuration details to generate combined hardware configurations
                // starts with 2
                int amplifierNumber = configurationDetails.Split(separator: ampDelimiter, options: StringSplitOptions.RemoveEmptyEntries).Length - 1;

                // initialize holder for amplifier numbers
                ObservableCollection<int> amplifierNumbers = new ObservableCollection<int>();
                // initialize the counter
                for (int ind = 1; ind <= amplifierNumber; ind++)
                {
                    // retrieve amplifier number(s) from the raw response of Omicron Test Set
                    amplifierNumbers.Add(Convert.ToInt32(configurationDetails.Remove(configurationDetails.LastIndexOf(';'), 1).Split(separator: ampDelimiter, options: StringSplitOptions.RemoveEmptyEntries)[ind]));
                }

                // split up the omicron response.
                string[] responses = configurationDetails.Split(separator: delimiterStrings, options: StringSplitOptions.RemoveEmptyEntries);

                // assign some visualization string values per "amplifier type"
                switch (amplifierType)
                {
                    // amplifier type is voltage
                    case "voltage":
                        // pick correct the amplifier type per Omicron documentation only eligible for voltage amplifiers are "1" and "5"
                        if (!Equals("1", responses.GetValue(responses.Length - 1)) && !Equals("5", responses.GetValue(responses.Length - 1)))
                        {
                            // not a voltage amplifier
                            continue;
                        }
                        // amplifier type is voltage
                        amplifierInitial = "V";
                        // output type is A
                        outputType = "A";
                        break;
                    // amplifier type is current
                    case "current":
                        // pick correct the amplifier type per Omicron documentation only eligible for current amplifiers are "2" and "6"
                        if (!Equals("2", responses.GetValue(responses.Length - 1)) && !Equals("6", responses.GetValue(responses.Length - 1)))
                        {
                            // not a current amplifier
                            continue;
                        }
                        // amplifier type is current
                        amplifierInitial = "A";
                        // output type is V
                        outputType = "V";
                        break;
                    default:
                        // update the log
                        IoC.Logger.Log($"Omicron amplifier {responses.GetValue(responses.Length - 1)} is not supported");
                        continue;
                }

                // add configuration id
                outputConfiguration.ConfigIDs.Add(Convert.ToInt32(responses[1]));
                // add phase count
                outputConfiguration.PhaseCounts.Add(Convert.ToInt32(responses[2]));
                // add amplifiers' maximum output
                outputConfiguration.MaxOutput.Add(Convert.ToDouble(responses[3]));
                // retrieve mode
                outputConfiguration.Mode = $"{responses[7]}";
                // retrieve wiring id.
                outputConfiguration.WiringID = Convert.ToInt32(responses[8]);
                // add file name
                outputConfiguration.WiringDiagramFileLocation = $"{outputConfiguration.Mode}{outputConfiguration.WiringID}";
                // add amplifiers' output signal
                outputConfiguration.AmplifierNumber = amplifierNumbers;
                // save raw response
                outputConfiguration.RawOmicronResponse = configurationDetails;

                // is output configuration has an automatically calculated resultant -- String == "zero"?
                string autamaticallyCalculated = string.Empty;

                // only mode == zero where v4 automatically calculated
                if (Equals(responses[7], "zero"))
                {
                    // two options Voltage is "V" and Current is "I"
                    autamaticallyCalculated = $"{(amplifierInitial == "A" ? "I" : amplifierInitial)}E automatically calculated";
                }

                // outputConfiguration.
                // 3x300V, 
                string magnitudeString = $"{responses[2]}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 85VA @ 85V,
                string vaString = $"{Convert.ToDouble(responses[4], CultureInfo.CurrentCulture)}VA @ {Convert.ToDouble(responses[5], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 3x300V, 85VA @ 85V, 1Arms
                outputConfiguration.WiringDiagramString = $"{magnitudeString}{vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{outputType}rms, {autamaticallyCalculated}";
                // add group name for the radio buttons
                outputConfiguration.GroupName = amplifierInitial;

                // construct the string.
                await IoC.Task.Run(() => IoC.Logger.Log(outputConfiguration.WiringDiagramString));

                // is this amplifier Selected Voltage or Selected Current?
                if (string.Equals(outputConfiguration.WiringDiagramFileName, IoC.Settings.SelectedVoltage) || string.Equals(outputConfiguration.WiringDiagramFileName, IoC.Settings.SelectedCurrent))
                {
                    // update check box
                    outputConfiguration.CurrentWiringDiagram = true;
                }

                // construct the view model.
                outputConfigurations.Add(outputConfiguration);

                // check this view model if view model can be combined with previous view model.
                switch (outputConfiguration.Mode)
                {
                    case "std":
                    case "ser12":
                    case "ser13":
                    case "gen2":
                    case "par1":
                    case "par2ser1": // not 100% sure if this mode can be combine.
                        // only single amplifiers can be combined
                        if (1 == amplifierNumber)
                        {
                            // add single amplifier to next single amplifier
                            outputConfigurationsToCombine.Add(outputConfiguration);

                            // combine two amplifiers if there are two of them
                            if (2 == outputConfigurationsToCombine.Count)
                            {
                                // the new phase count supported by the combined amplifiers.
                                int phaseCount = default;

                                // generate new combination phase count
                                ObservableCollection<int> phaseCounts = new ObservableCollection<int>();
                                // reverse order to match Omicron Hardware UI
                                for (int b = outputConfigurationsToCombine.Count - 1; b >= 0; b--)
                                {
                                    // retrieve Phase Counts 
                                    foreach (var item in outputConfigurationsToCombine[b].PhaseCounts)
                                    {
                                        // store new phase counts for combined configuration
                                        phaseCounts.Add(item);
                                        // this is value use in UI
                                        phaseCount += item;
                                    }
                                }

                                // generate new combination max output
                                ObservableCollection<double> maxOutput = new ObservableCollection<double>();
                                // reverse order to match Omicron Hardware UI
                                for (int b = outputConfigurationsToCombine.Count - 1; b >= 0; b--)
                                {
                                    // retrieve Maximum Outputs
                                    foreach (var item in outputConfigurationsToCombine[b].MaxOutput)
                                    {
                                        // store new maximum output for combined configuration
                                        maxOutput.Add(item);
                                    }
                                }

                                // generate new combination configuration ids
                                ObservableCollection<int> configIds = new ObservableCollection<int>();
                                // reverse order to match Omicron Hardware UI
                                for (int b = outputConfigurationsToCombine.Count - 1; b >= 0; b--)
                                {
                                    // retrieve configuration identifications
                                    foreach (var item in outputConfigurationsToCombine[b].ConfigIDs)
                                    {
                                        // store new configuration identifications for combined configuration
                                        configIds.Add(item);
                                    }
                                }

                                // generate new combination amplifiers
                                ObservableCollection<int> amplifiers = new ObservableCollection<int>();
                                // reverse order to match Omicron Hardware UI
                                for (int b = outputConfigurationsToCombine.Count - 1; b >= 0; b--)
                                {
                                    // retrieve amplifier numbers
                                    foreach (var item in outputConfigurationsToCombine[b].AmplifierNumber)
                                    {
                                        // store new amplifier numbers for combined configuration
                                        amplifiers.Add(item);
                                    }
                                }
                                
                                // add new combination configuration to the list.
                                SettingsListItemViewModel settings = new SettingsListItemViewModel()
                                {
                                    // configuration ids for the combined amplifiers
                                    ConfigIDs = configIds,
                                    // max output of the combination of the amplifiers
                                    MaxOutput = maxOutput,
                                    // phase counts for the combined amplifiers
                                    PhaseCounts = phaseCounts,
                                    // wire diagram file location to show to the user
                                    WiringDiagramFileLocation = $"{outputConfigurationsToCombine[1].Mode}{outputConfigurationsToCombine[1].WiringID}{outputConfigurationsToCombine[0].Mode}{outputConfigurationsToCombine[0].WiringID}",
                                    // construct string to show to the user.
                                    WiringDiagramString = $"{phaseCount}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierInitial}, {vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{outputType}rms",
                                    // add group name for the radio buttons
                                    GroupName = amplifierInitial,
                                    // add the mode since both amplifiers has same mode just use the last ones
                                    Mode = outputConfigurationsToCombine[1].Mode,
                                    // save raw response
                                    RawOmicronResponse = $"{outputConfigurationsToCombine[1].RawOmicronResponse}, {outputConfigurationsToCombine[0].RawOmicronResponse}",
                                    // new Amplifier Descriptors
                                    AmplifierNumber = amplifiers,
                                };

                                // generate file name to compare selected configuration file name.
                                string wiringDiagramFileName = $"{outputConfigurationsToCombine[1].Mode}{outputConfigurationsToCombine[1].WiringID}{outputConfigurationsToCombine[0].Mode}{outputConfigurationsToCombine[0].WiringID}";
                                // update check box status
                                settings.CurrentWiringDiagram = string.Equals(wiringDiagramFileName, IoC.Settings.SelectedVoltage) || string.Equals(wiringDiagramFileName, IoC.Settings.SelectedCurrent);

                                // add combined hardware configuration to the list
                                outputConfigurations.Add(settings);

                                // update the log.
                                await IoC.Task.Run(() => IoC.Logger.Log(outputConfigurations.Last().WiringDiagramString));
                            }
                        }
                        break;
                    default:
                        // reset combination list.
                        outputConfigurationsToCombine.Clear();
                        break;
                }
            }
            // return the view model in reverse order to match Omicron Hardware Configuration interface.
            return new ObservableCollection<SettingsListItemViewModel>(outputConfigurations.Reverse());
        }

        #endregion
    }
}
