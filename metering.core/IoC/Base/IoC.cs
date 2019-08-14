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
        /// Shortcut to access the <see cref="IUIManager"/>
        /// </summary>
        public static IUIManager UI => IoC.Get<IUIManager>();

        /// <summary>
        /// A shortcut to the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel Application => IoC.Get<ApplicationViewModel>();

        /// <summary>
        /// A shortcut to access the <see cref="NominalValuesViewModel"/>
        /// </summary>
        public static NominalValuesViewModel NominalValues => IoC.Get<NominalValuesViewModel>();

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
