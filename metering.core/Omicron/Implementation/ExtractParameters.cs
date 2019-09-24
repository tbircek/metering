namespace metering.core
{
    /// <summary>
    /// Extract parameters from Omicron Test Set's responses
    /// </summary>
    public struct ExtractParameters
    {
        /// <summary>
        /// Returns Omicron Test Set response value per user specified location of parameter.
        /// </summary>
        /// <param name="parameterLocation">Location of the parameter in Omicron Test Set response.</param>
        /// <param name="omicronCommandResponse">String response from Omicron Test Set.</param>
        public string Parameters(int parameterLocation, string omicronCommandResponse)
        {
            // check if parameter exists in response
            if (parameterLocation <= 0)
                return "No response in specified omicronCommandResponse string.";
            
            // check if response is blank
            if (string.IsNullOrWhiteSpace(omicronCommandResponse))
                // no response found.
                return "There was either no response or a blank string from Omicron Test Set.";
           
            // Parameter index is based 0
            string[] response = omicronCommandResponse.Split(',');

            // return specified parameter location (0 based)
            return response[parameterLocation - 1];

        }

        /// <summary>
        /// Future use.
        /// </summary>
        public void Parameters(string CmdResult, string CmdSend)
        {
            // inform developer
            IoC.Logger.Log($"Omicron response: {CmdResult}\tCommand send: {CmdSend}",LogLevel.Informative);
        }
    }
}