using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace metering.core
{
    /// <summary>
    /// Detects harmonics test orders if a harmonics test available
    /// </summary>
    public class TestHarmonics
    {

        /// <summary>
        /// Holds harmonics orders.
        /// </summary>
        /// <returns>Returns a Tuple with harmonics orders</returns>       
        //public (List<int> HarmonicOrderStart, List<int> HarmonicOrderEnd, bool IsHarmonicTestAvailable, List<string> HarmonicOrders) GetHarmonicOrder()
        public (List<int> HarmonicOrderStart, List<int> HarmonicOrderEnd, List<string> HarmonicOrders, int TotalHarmonicTestCount) GetHarmonicOrder()

        {
            // initialize Tuple variables with default values
            List<int> HarmonicOrderStart = new List<int>();
            List<int> HarmonicOrderEnd = new List<int>();
            int TotalHarmonicTestCount = default;

            // generate Harmonics Order test even it is not a harmonics test
            List<string> HarmonicOrders = IoC.TestDetails.HarmonicsOrder.Split(',').ToList();

            if (IoC.TestDetails.IsHarmonics)
            {
                
                // scan harmonic orders to decide what orders are due to test
                foreach (var harmonicOrder in HarmonicOrders)
                {
                    // "-" gives a range e.g. 2-5 => means 2,3,4, and 5 orders
                    if (harmonicOrder.Split('-').Count() == 1)
                    {
                        // no dash so this is a single harmonic order test
                        HarmonicOrderStart.Add(Convert.ToInt16(harmonicOrder.ToString()));
                        HarmonicOrderEnd.Add(Convert.ToInt16(harmonicOrder.ToString()));
                        // update harmonic test counter for progress calculations.
                        TotalHarmonicTestCount += 1;
                    }
                    // there is a "-" available
                    else if (harmonicOrder.Split('-').Count() == 2)
                    {
                        // first item is starting harmonic order
                        HarmonicOrderStart.Add(Convert.ToInt16(harmonicOrder.Split('-')[0].ToString()));
                        // second item is ending harmonic order
                        HarmonicOrderEnd.Add(Convert.ToInt16(harmonicOrder.Split('-')[1].ToString()));
                        // update harmonic test counter for progress calculations.
                        TotalHarmonicTestCount += Convert.ToInt16(harmonicOrder.Split('-')[1].ToString()) - Convert.ToInt16(harmonicOrder.Split('-')[0].ToString()) + 1;
                    }
                    // bad input format
                    else
                    {
                        // inform the user 
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: Harmonics Order is NOT valid. Please fix it.";
                        // update harmonic test counter for progress calculations.
                        TotalHarmonicTestCount = 0;
                        // stop testing if there is a bad input
                        //IsHarmonicTestAvailable &= false;
                        // exit loop
                        break;
                    }
                }
                // return properties of the harmonic order found
                return (HarmonicOrderStart, HarmonicOrderEnd, HarmonicOrders, TotalHarmonicTestCount);
                //return (HarmonicOrderStart, HarmonicOrderEnd, IsHarmonicTestAvailable, HarmonicOrders);
            }
            else
            {
                // no harmonics found
                HarmonicOrderStart.Add(Convert.ToInt16(HarmonicOrders[0].ToString()));
                HarmonicOrderEnd.Add(Convert.ToInt16(HarmonicOrders[0].ToString()));

                // update harmonic test counter for progress calculations.
                TotalHarmonicTestCount += 1;

                // return properties of no harmonic order found
                return (HarmonicOrderStart, HarmonicOrderEnd, HarmonicOrders, TotalHarmonicTestCount);
                //return (HarmonicOrderStart, HarmonicOrderEnd, IsHarmonicTestAvailable, HarmonicOrders);
            }
        }
    }
}