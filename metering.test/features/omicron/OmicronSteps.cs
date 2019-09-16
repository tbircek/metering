using System.Diagnostics;
using metering.core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace meteringspecs.features.omicron
{
    [Binding]
    public class OmicronSteps
    {
        CMCControl engine = new CMCControl();

        [Given(@"I start the application")]
        public void GivenIStartTheApplication()
        {
            Debug.WriteLine("Application is running ;)");
        }

        [Given(@"I have Omicron Test Set available on network")]
        public void GivenIHaveOmicronTestSetAvailableOnNetwork()
        {
            Assert.AreEqual(true, engine.FindCMC());
        }

        [Given(@"I have a DeviceID")]
        public void GivenIHaveADeviceID()
        {
            // this result also shows that extract parameters worked correctly.
            Assert.IsNotNull(engine.DeviceID);
        }

        [Given(@"I have initialSetup is successful")]
        public void GivenIHaveInitialSetupIsSuccessfull()
        {
            GivenIHaveOmicronTestSetAvailableOnNetwork();
            ThenTheResultShouldBeAOkOnTheScreen();

        }

        [When(@"I have press connect")]
        public void WhenIHavePressConnect()
        {
            // was InitialSetup success?
            Assert.IsTrue(engine.FindCMC());
        }

        [Then(@"the result should be a DeviceID on the screen")]
        public void ThenTheResultShouldBeAOnTheScreen()
        {
            Assert.AreEqual(1, engine.DeviceID);
        }

        [Then(@"the result should be a OK on the screen")]
        public void ThenTheResultShouldBeAOkOnTheScreen()
        {
            // initial setup run?
            engine.InitialSetup();
            Assert.AreEqual(1, engine.DeviceID);
        }

        [Then(@"the result should be Omicron Test Set to power up")]
        public async void ThenTheResultShouldBeOmicronTestSetToPowerUp()
        {
            // manual observation?
            //engine.TurnOnCMC();
            await engine.TestSampleAsync(Register: 2279,
                              From: 4.0 * 100.0 / 120.0,
                              To: 4.0 * 135.0 / 120.0,
                              Delta: 4.0 * 7.0 / 120.0,
                              DwellTime: 15,
                              MeasurementDuration: 0,
                              StartDelayTime: 5,
                              MeasurementInterval: 50,
                              StartMeasurementDelay: 10,
                              message: string.Empty
                              );
        }

        [Then(@"Omicron Test Set should be power down")]
        public void ThenOmicronTestSetShouldBePowerDown()
        {
            // manual observation?
            engine.TurnOffCMC();
        }
    }
}
