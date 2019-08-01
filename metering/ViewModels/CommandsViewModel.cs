using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;

namespace metering
{
    [AddINotifyPropertyChangedInterface]
    public class CommandsViewModel : BaseViewModel
    {

        #region Public Commands

        /// <summary>
        /// The command to handle change view to test plan detail view
        /// and populate items with nominal values
        /// </summary>
        public ICommand AddNewTestCommand { get; set; }


        /// <summary>
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        /// <summary>
        /// NominalValuesViewModel to access its values.
        /// </summary>
        private NominalValuesViewModel nominalValues { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {
            AddNewTestCommand = new RelayCommand(param => CopyNominalValues());
            CancelNewTestCommand = new RelayCommand(CancelNominalValues);
        }

        #endregion

        #region Helpers

        private void CancelNominalValues()
        {
            // throw new NotImplementedException();
            Debug.WriteLine("CancelNominalValues is running:");
            // testDetailsModel.TestDetail.Clear();
        }

        private void CopyNominalValues()
        {
            // TestDetailsModel testDetailsModel = new TestDetailsModel();

            // throw new NotImplementedException();
            Debug.WriteLine("Following values reported:");

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronVoltageOutputNumber = 4;
            for (int i = 1; i <= omicronVoltageOutputNumber; i++)
            {
                string[] phase;
                if (nominalValues.SelectedVoltagePhase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0" };
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0" };
                }

                Debug.WriteLine($"signal: v{i}\tfrom: {nominalValues.NominalVoltage}\tto: {nominalValues.NominalVoltage}\tdelta: {nominalValues.NominalDelta}\tphase: {phase[i - 1]}\tfrequency: {nominalValues.NominalFrequency}");
                //TestDetailModel test = new TestDetailModel
                //{
                //    SignalName = "v" + i,
                //    From = model.Voltage,
                //    To = model.Voltage,
                //    Delta = model.Delta,
                //    Phase = phase[i - 1],
                //    Frequency = model.Frequency
                //};
                ////testDetailsModel.TestDetail.Add(test);
                ///
                //Test test = new Test(
                //signalName: "v" + i,
                //        from: model.Voltage,
                //        to: model.Voltage,
                //        delta: model.Delta,
                //        phase: phase[i - 1],
                //        frequency: model.Frequency
                //    );

                //testDetails.Add(test);

            }

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronCurrentOutputNumber = 6;
            for (int i = 1; i <= omicronCurrentOutputNumber; i++)
            {
                string[] phase;
                if (nominalValues.SelectedCurrentPhase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0", "-120", "120" };
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0", "0", "0" };
                }

                Debug.WriteLine($"signal: i{i}\tfrom: {nominalValues.NominalCurrent}\tto: {nominalValues.NominalCurrent}\tdelta: {nominalValues.NominalDelta}\tphase: {phase[i - 1]}\tfrequency: {nominalValues.NominalFrequency}");
                //TestDetailModel test = new TestDetailModel
                //{
                //    SignalName = "i" + i,
                //    From = model.Current,
                //    To = model.Current,
                //    Delta = model.Delta,
                //    Phase = phase[i - 1],
                //    Frequency = model.Frequency
                //};
                ////testDetailsModel.TestDetail.Add(test);

                //Test test = new Test(
                //signalName: "i" + i,
                //           from: model.Current,
                //           to: model.Current,
                //           delta: model.Delta,
                //           phase: phase[i - 1],
                //           frequency: model.Frequency
                //       );

                //testDetails.Add(test);
            }
            Debug.WriteLine("TODO: show new TestDetailsView");

            // testDetailsModel.TestDetail = testDetails;
            //testDetailsModel = new TestDetailsModel("", "", "", "", "", "", testDetails);
            // detailsModel.TestDetail.Add()
            //TestDetailsViewModel testDetailsViewModel = new TestDetailsViewModel("", "", "", "", "", "", testDetailsModel.TestDetail);

            #endregion

        }

    }
}
