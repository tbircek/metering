using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace metering
{
    public struct ExtractParameters
    {
        /// <summary>
        /// Returns Omicron Test Set response value per user specified location of parameter.
        /// </summary>
        /// <param name="parameterLocation">Location of the parameter in Omicron Test Set response.</param>
        /// <param name="omicronCommandResponse">String response from Omicron Test Set.</param>
        public string Parameters(int parameterLocation, string omicronCommandResponse)
        {
            if (parameterLocation <= 0)
            {
                return "No response in specified omicronCommandResponse string.";
            }

            if (string.IsNullOrWhiteSpace(omicronCommandResponse))
            {
                return "There was either no response or a blank string from Omicron Test Set.";
            }

            // Parameter index is based 0
            string[] response = omicronCommandResponse.Split(',');
            return response[parameterLocation - 1];

        }

        /// <summary>
        /// Future use.
        /// </summary>
        public void Parameters(string CmdResult, string CmdSend)
        {
            Debug.WriteLine(string.Format("Omicron response: {0}\tsend cmd: {1}", CmdResult, CmdSend));
        }
    }
}