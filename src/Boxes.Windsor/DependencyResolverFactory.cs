namespace Boxes.Windsor
{
    using System;
    using Castle.Windsor;
    using Integration;
    using Integration.Factories;

    internal class DependencyResolverFactory : IDependencyResolverFactory
    {
        public IDependencyResolver CreateResolver(object container)
        {
            var windsorContainer = container as IWindsorContainer;
            if (windsorContainer == null)
            {
                throw new Exception("container needs to be a WindsorContaienr");
            }

            return new DependencyResolver(windsorContainer);
        }
    }
}