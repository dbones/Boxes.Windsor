namespace Boxes.Windsor
{
    using Castle.Windsor;
    using Integration.Factories;

    internal class IocFactory : IIocFactory<IWindsorContainer,IWindsorContainer>
    {
        private readonly IIocSetup<IWindsorContainer> _setup;

        public IocFactory(IIocSetup<IWindsorContainer> setup)
        {
            _setup = setup;
        }

        public IWindsorContainer CreateBuilder()
        {
            var container = new WindsorContainer();
            _setup.Configure(container);
            return container;
        }

        public IWindsorContainer CreateBuilder(IWindsorContainer parentContainer)
        {
            var container = new WindsorContainer();
            parentContainer.AddChildContainer(container);
            _setup.ConfigureChild(container);
            return container;
        }

        public IWindsorContainer CreateContainer(IWindsorContainer builder)
        {
            return builder;
        }

        public IWindsorContainer CreateChildContainer(IWindsorContainer container, IWindsorContainer builder)
        {
            return builder;
        }
    }
}