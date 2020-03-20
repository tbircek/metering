using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metering.core
{
    /// <summary>
    /// Calculates Standard deviation of the given data.
    /// </summary>
    public class CalculateStdDeviation
    {

        /// <summary>
        /// Calculates Standard Deviation.
        /// See <see cref="https://www.mathsisfun.com/data/standard-deviation-formulas.html"/> for more information.
        /// </summary>
        /// <param name="AllValues">All legit modbus register readings available.</param>
        /// <param name="TestValue">Test value for the reporting for these modbus readings.</param>
        public void CalculateStdDeviationWithTime(List<SortedDictionary<DateTime, int>> AllValues, double TestValue)
        {

            // retrieved general information of the current register.
            foreach (SortedDictionary<DateTime, int> registerReading in AllValues)
            {
                // generate fixed portion of header information for reporting.
                StringBuilder standardDeviationString = new StringBuilder($"Time, Test Value, Value Read, {string.Empty}").AppendLine();

                // update Minimum value.
                IoC.CMCControl.MinValues.SetValue(registerReading.Values.Min(), AllValues.IndexOf(registerReading));
                // System.Diagnostics.Debug.WriteLine($"Min value: {MinValues.GetValue(AllValues.IndexOf(registerReading))}");
                // update Maximum value.
                IoC.CMCControl.MaxValues.SetValue(registerReading.Values.Max(), AllValues.IndexOf(registerReading));
                // System.Diagnostics.Debug.WriteLine($"Max value: {MaxValues.GetValue(AllValues.IndexOf(registerReading))}");
                // update Average value.
                IoC.CMCControl.Averages.SetValue(registerReading.Values.Average(), AllValues.IndexOf(registerReading));
                // System.Diagnostics.Debug.WriteLine($"Mean value: {Averages.GetValue(AllValues.IndexOf(registerReading))}");
                // update GoodReading value.
                IoC.CMCControl.SuccessCounters.SetValue(registerReading.Count(), AllValues.IndexOf(registerReading));
                // System.Diagnostics.Debug.WriteLine($"GoodReading value: {SuccessCounters.GetValue(AllValues.IndexOf(registerReading))}");
                // update Total value.
                IoC.CMCControl.Totals.SetValue(registerReading.Values.Sum(), AllValues.IndexOf(registerReading));
                // System.Diagnostics.Debug.WriteLine($"Total value: {Totals.GetValue(AllValues.IndexOf(registerReading))}");

                // new list to hold square of the differences
                List<double> squaredDifferences = new List<double>();

                // Step 2. Then for each number: subtract the Mean and square the result
                foreach (KeyValuePair<DateTime, int> entry in registerReading)
                {
                    // add up all the squared values 
                    squaredDifferences.Add(Math.Pow(entry.Value - registerReading.Values.Average(), 2));

                    standardDeviationString.AppendLine(value: $"{entry.Key:MM/dd/yy HH:mm:ss.fff},{TestValue:F6},{entry.Value:F6}, {string.Empty}");
                }

                // Step 3. add up all the values then divide by how many.
                double sigmaNotation = squaredDifferences.Average();

                // Step 4. Take the square root of that.
                double standardDeviation = Math.Pow(sigmaNotation, 0.5);

                // update Standard Deviation value.
                IoC.CMCControl.StandardDeviations.SetValue(standardDeviation, AllValues.IndexOf(registerReading));


                // check if the user wants to save modbus reading details.
                if (IoC.Communication.IsSaveHoldingRegisterDetailsChecked)
                {

                    // report file id to distinguish between test results 
                    string fileId = $"{DateTime.Now.ToLocalTime():yyyy_MM_dd_HH_mm}";

                    // initialize test details string.
                    string testDetailsFileName = string.Empty;

                    if (string.IsNullOrWhiteSpace(IoC.Communication.CurrentTestFileListItem.TestFileNameWithExtension))
                    {
                        // test result file name contains Register, From, To, and test start time values
                        testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.TestDetails.Register}_{IoC.CMCControl.StandardDeviations:F6}-{IoC.CMCControl.StandardDeviations:F6}_{fileId}";
                    }
                    else
                    {
                        // test result file name contains "Test File Name" per the user input.
                        // file name might contain multiple "."
                        testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.Communication.CurrentTestFileListItem.ShortTestFileName}_{IoC.TestDetails.Register.ToString().Split(',').GetValue(AllValues.IndexOf(registerReading))}_{fileId}";
                    }

                    // access previous readings
                    StringBuilder builder = IoC.CMCControl.IndivudalRegisters.ElementAt(AllValues.IndexOf(registerReading));

                    // retrieve previous reading if there any
                    List<string> oldText = builder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // retrieve current reading per register
                    List<string> newText = standardDeviationString.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // temporary storage to keep values
                    StringBuilder tempBuilder = new StringBuilder();

                    // decide the max message length
                    int length = Math.Max(oldText.Count, newText.Count);

                    // is this the first reading?
                    if (1 < oldText.Count)
                    {
                        // are previous record same length as the current readings?
                        if (length > oldText.Count)
                        {
                            //// this is a one record.
                            //IEnumerable<string> oneRecord = Enumerable.Repeat($"{string.Empty},{string.Empty},{string.Empty},{string.Empty}", length - oldText.Count);

                            // not same size. increase size
                            oldText.Add(new StringExtensions().RepeatStringBuilderInsert(",,,", length - oldText.Count));
                        }

                        // are new records same length as the previous readings?
                        if (length > newText.Count)
                        {
                            //// this is a one record.
                            //IEnumerable<string> oneRecord = Enumerable.Repeat($"{string.Empty},{string.Empty},{string.Empty},{string.Empty}", length - newText.Count);


                            // not same size. increase size
                            newText.Add(new StringExtensions().RepeatStringBuilderInsert(",,,", length - newText.Count));
                        }

                        // step through every records
                        for (int i = 0; i < length; i++)
                        {
                            // stitch each old record with the new record
                            tempBuilder.AppendLine($"{oldText.ElementAt(i)},{newText.ElementAt(i)}");
                        }

                        // remove previous reading
                        IoC.CMCControl.IndivudalRegisters.RemoveAt(AllValues.IndexOf(registerReading));
                        // add new reading
                        IoC.CMCControl.IndivudalRegisters.Insert(AllValues.IndexOf(registerReading), tempBuilder);
                    }
                    // this is the first reading
                    else
                    {
                        // remove previous reading
                        IoC.CMCControl.IndivudalRegisters.RemoveAt(AllValues.IndexOf(registerReading));
                        // add new reading
                        IoC.CMCControl.IndivudalRegisters.Insert(AllValues.IndexOf(registerReading), standardDeviationString);
                    }
                }
            }
        }
    }

    /// <summary>
    /// adds extra string methods
    /// </summary>
    public class StringExtensions
    {
        /// <summary>
        /// Repeats specified string n times
        /// </summary>
        /// <param name="s">string to repeat</param>
        /// <param name="n">number times to repeat</param>
        /// <returns>new string builder with n times s.</returns>
        public string RepeatStringBuilderInsert(string s, int n)
        {
            // returns new string builder
            return new StringBuilder(s.Length * n)
                        .Insert(0, s, n)
                        .ToString();
        }
    }
}