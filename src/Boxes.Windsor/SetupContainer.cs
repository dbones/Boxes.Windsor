namespace Boxes.Windsor
{
    using Castle.Windsor;
    using Integration.Extensions;
    using Integration.Setup;
    using Integration.Setup.Interception;

    /// <summary>
    /// Setup a Windser container.
    /// </summary>
    public abstract class SetupContainer : ISetupBoxesExtension<IDefaultContainerSetup<IWindsorContainer>> 
    {
        public bool CanHandle(IDefaultContainerSetup<IWindsorContainer> extension)
        {
            return true;
        }

        /// <summary>
        /// create a new regiser pattern
        /// </summary>
        public Register Register { get {return new Register();}}

        /// <summary>
        /// create a new interception (AOP) pattern
        /// </summary>
        public IRegisterInterception RegisterInterception { get { return new RegisterInterception(); } }

        public abstract void Configure(IDefaultContainerSetup<IWindsorContainer> extension);
    }
}