using Ninject;

namespace metering.core
{
    /// <summary>
    /// IoC container for the application
    /// </summary>
    public static class IoC
    {
        #region Public Properties

        /// <summary>
        /// The kernel for IoC container
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// A shortcut to the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="NominalValuesViewModel"/>
        /// </summary>
        public static NominalValuesViewModel NominalValues => Get<NominalValuesViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="CommandsViewModel"/>
        /// </summary>
        public static CommandsViewModel Commands => Get<CommandsViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="TestDetailsViewModel"/>
        /// </summary>
        public static TestDetailsViewModel TestDetails => Get<TestDetailsViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="CommunicationViewModel"/>
        /// </summary>
        public static CommunicationViewModel Communication => Get<CommunicationViewModel>();

        /// <summary>
        /// A shortcut to access to <see cref="CMCControl"/>
        /// </summary>
        public static CMCControl CMCControl => Get<CMCControl>();

        /// <summary>
        /// A shortcut to access to <see cref="StringCommands"/>
        /// </summary>
        public static StringCommands StringCommands => Get<StringCommands>();

        /// <summary>
        /// A shortcut to access to <see cref="InitialCMCSetup"/>
        /// </summary>
        public static InitialCMCSetup InitialCMCSetup => Get<InitialCMCSetup>();

        /// <summary>
        /// A shortcut to access to <see cref="FindCMC"/>
        /// </summary>
        public static FindCMC FindCMC => Get<FindCMC>();

        /// <summary>
        /// A shortcut to access to <see cref="PowerOptions"/>
        /// </summary>
        public static PowerOptions PowerOptions => Get<PowerOptions>();

        #endregion

        #region Setup

        /// <summary>
        /// Setups the IoC container, binds all information required and is ready to use
        /// NOTE: Must be called as soon as application starts up to ensure all services can be used
        /// </summary>
        public static void Setup()
        {
            // Bind all required view models
            BindViewModels();

            // Bind all required classes
            BindClasses();
        }

        /// <summary>
        /// Binds single instance of the classes
        /// </summary>
        private static void BindClasses()
        {
            // Bind a single instance of CMCControl class
            Kernel.Bind<CMCControl>().ToConstant(new CMCControl());

            // Bind a single instance of Omicron StringCommands
            Kernel.Bind<StringCommands>().ToConstant(new StringCommands());

            // Bind a single instance of Omicron InitialCMCSetup
            Kernel.Bind<InitialCMCSetup>().ToConstant(new InitialCMCSetup());

            // Bind a single instance of Omicron FindCMC
            Kernel.Bind<FindCMC>().ToConstant(new FindCMC());

            // Bind a single instance of Omicron PowerOptions
            Kernel.Bind<PowerOptions>().ToConstant(new PowerOptions());
        }

        /// <summary>
        /// Binds to a single instance of view models
        /// </summary>
        private static void BindViewModels()
        {
            // Bind to a single instance of Application view model
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());

            // bind to a single instance of NominalValues view model
            Kernel.Bind<NominalValuesViewModel>().ToConstant(new NominalValuesViewModel());

            // bind to a single instance of CommandsViewModel view model
            Kernel.Bind<CommandsViewModel>().ToConstant(new CommandsViewModel());

            // bind to a single instance of TestDetailsViewModel
            Kernel.Bind<TestDetailsViewModel>().ToConstant(new TestDetailsViewModel());

            // bind to a single instance of CommunicationViewModel
            Kernel.Bind<CommunicationViewModel>().ToConstant(new CommunicationViewModel());
        }

        #endregion

        #region Public Functions
        /// <summary>
        /// Gets a service from IoC of the specified type
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }

        #endregion
    }
}
