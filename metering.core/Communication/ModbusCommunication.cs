using System;
using System.IO.Ports;

namespace metering.core
{
    public class ModbusClient
    {

        private EasyModbus.ModbusClient modbusClient;

        /// <summary>
        /// Constructor for serial connection. For future implementation.
        /// </summary>
        /// <param name="serialPort">name of the serial port to be used</param>
        public ModbusClient(string serialPort)
        {
            throw new System.NotImplementedException("Modbus Serial protocol is not implemented");
        }

        /// <summary>
        /// use properties to specify the client
        /// </summary>
        public ModbusClient()
        {
            try
            {
                modbusClient = new EasyModbus.ModbusClient();
            }
            catch (EasyModbus.Exceptions.ModbusException ex)
            {
                // TODO: Log this error in to log file.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: ModbusException error. {ex.Message}.\n";
            }
        }

        /// <summary>
        /// Constructor for Modbus TCP and Modbus UDP connection.
        /// </summary>
        /// <param name="ipAddress">Ip Address of the server</param>
        /// <param name="port">port of the server</param>
        public ModbusClient(string ipAddress, int port)
        {
            try
            {
                modbusClient = new EasyModbus.ModbusClient(ipAddress, port);
            }
            catch (EasyModbus.Exceptions.ConnectionException)
            {
                // TODO: Log this error in to log file.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Connection error. Please check connection.\n";
            }
        }

        /// <summary>
        /// returns “TRUE” if client is connected to Server; otherwise “FALSE”
        /// </summary>
        public bool GetConnected()
        {
            return modbusClient.Connected;
        }

        /// <summary>
        /// Gets or Sets the IP-Address of the Server to be connected
        /// </summary>
        public string IpAddress
        {
            get
            {
                return modbusClient.IPAddress;
            }
            set
            {
                modbusClient.IPAddress = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Port were the Modbus-TCP Server is reachable
        /// </summary>
        public int Port
        {
            get
            {
                return modbusClient.Port;
            }
            set
            {
                modbusClient.Port = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Unit identifier in case of serial connection (Default = 0)
        /// For future implementation.
        /// </summary>
        public byte UnitIdentifier
        {
            get
            {
                throw new System.NotImplementedException("Modbus Serial protocol is not implemented");
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the Baudrate for serial connection (Default = 9600)
        /// For future implementation.
        /// </summary>
        public int Baudrate
        {
            get
            {
                throw new System.NotImplementedException("Modbus Serial protocol is not implemented");
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the of Parity in case of serial connection
        /// For future implementation.
        /// </summary>
        public Parity Parity
        {
            get
            {
                throw new System.NotImplementedException("Modbus Serial protocol is not implemented");
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the number of stopbits in case of serial connection
        /// For future implementation.
        /// </summary>
        public StopBits StopBits
        {
            get
            {
                throw new System.NotImplementedException("Modbus Serial protocol is not implemented");
            }
            set
            {
            }
        }

        /// <summary>
        /// Gets or Sets the connection Timeout in case of ModbusTCP connection
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                return modbusClient.ConnectionTimeout;
            }
            set
            {
                modbusClient.ConnectionTimeout = value;
            }
        }
        /// <summary>
        /// Connects to the Modbus Server
        /// </summary>
        public void Connect()
        {
            try
            {
                modbusClient.Connect();
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                // TODO: Log this error in to log file.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Connection error. {ex.Message}.\n";
            }
            
        }

        /// <summary>
        /// Connects to the specified Modbus Server
        /// </summary>
        /// <param name="ipAddress">Ip Address of the server</param>
        /// <param name="port">port of the server</param>
        public void Connect(string ipAddress, int port)
        {
            try
            {
                modbusClient.Connect(ipAddress, port);
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                // TODO: Log this error in to log file.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Connection error. {ex.Message}.\n";
            }
        }

        /// <summary>
        /// Read Holding Registers from Master device (Function code 3)
        /// </summary>
        /// <param name="startingAddress">the first holding register address to be read</param>
        /// <param name="quantity">number of holding registers to be read</param>
        public int[] ReadHoldingRegisters(int startingAddress, int quantity)
        {
            try
            {
                if (!modbusClient.Connected)
                {
                    return null;
                }
                else
                {
                    return modbusClient.ReadHoldingRegisters(startingAddress - 1, quantity);
                }
            }
            catch (Exception ex)
            {
                // TODO: Log this error in to log file.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Exception occurred. {ex.Message}.\n";
                return null;
            }
        }

        /// <summary>
        /// Write single Register to Master device (Function code 6)
        /// </summary>
        /// <param name="startingAddress">register to be written</param>
        /// <param name="value">value to be written</param>
        public void WriteSingleRegister(int startingAddress, int value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Write multiple registers to Master device (Function code 16)
        /// </summary>
        /// <param name="startingAddress">first register to be written</param>
        /// <param name="values">Register Values [0..quantity-1] to be written</param>
        public void WriteMultipleRegisters(int startingAddress, int[] values)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (modbusClient.Connected)
                {
                    modbusClient.Disconnect();
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}
