using System;
using TechTalk.SpecFlow;
using metering.core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace meteringspecs.features.communication
{
    [Binding]
    public class CommunicationSteps
    {
        private readonly EasyModbus.ModbusClient mdbus = IoC.Communication.EAModbusClient;

        [Given(@"I have entered a connectible IpAddress")]
        public void GivenIHaveEnteredAConnectableIpaddress()
        {
            mdbus.IPAddress = "192.168.0.122";
        }
        
        [Given(@"I have entered connectible modbus port")]
        public void GivenIHaveEnteredConnectableModbusPort()
        {
            mdbus.Port = 502;
        }
        
        [Given(@"I have connection to the client")]
        public void GivenIHaveConnectionToTheClient()
        {
            Assert.IsTrue(mdbus.Connected);
        }
        
        [Given(@"I have entered a non-connectible IpAddress")]
        public void GivenIHaveEnteredANon_ConnectableIpaddress()
        {
            mdbus.ConnectionTimeout = 20000;
            mdbus.IPAddress = "192.168.0.1";
        }
        
        [Given(@"I have entered non-connectible modbus port")]
        public void GivenIHaveEnteredNon_ConnectableModbusPort()
        {
            mdbus.ConnectionTimeout = 20000;
            mdbus.Port = 1111;
        }
        
        [When(@"I press connect")]
        public void WhenIPressConnect()
        {
            mdbus.ConnectionTimeout = 20000;
            mdbus.Connect();
        }
        
        [When(@"I press disconnect")]
        public void WhenIPressDisconnect()
        {
            mdbus.Disconnect();
        }
        
        [Then(@"the result should be connected")]
        public void ThenTheResultShouldBeConnected()
        {
            Assert.IsTrue(mdbus.Connected);
        }
        
        [Then(@"the result should be a TimeOut")]
        public void ThenTheResultShouldBeATimeOut()
        {
            Assert.IsFalse(mdbus.Connected);
        }

        [Then(@"the result should be some numbers")]
        public void ThenTheResultShouldBeInt()
        {
            try
            {
                int[] serverResponse = mdbus.ReadHoldingRegisters(2279, 3);
                for (int i = 0; i < serverResponse.Length; i++)
                {
                    Debug.WriteLine(string.Format("{0} value {1}", 2279 + i , serverResponse[i]));

                }
                WhenIPressDisconnect();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
