namespace metering.viewModel
{
    class RegisterViewModel : ViewModelBase
    {
        private string register;
        private string progress;

        public string Register
        {
            get => register;
            set => SetProperty(ref register, value);
        }

        public string Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }
    }
}
