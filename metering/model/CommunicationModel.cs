namespace metering
{
    public class CommunicationModel
    {
        public string IpAddress { get; set; } = "192.168.0.122";
        public string Port { get; set; } = "502";
        public string Log { get; set; } = "";
        
        public CommunicationModel GetCommunicationModel()
        {
            return new CommunicationModel();
        }

        public CommunicationModel GetCommunicationModel( string ipAddress, string port, string log)
        {
            return new CommunicationModel
            {
                IpAddress = ipAddress,
                Port = port,
                Log  = log
            };
        }

        // TODO: Implement IDataErrorInfo
    }
}
