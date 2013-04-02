// Copyright 2012 - 2013 dbones.co.uk (David Rundle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace Boxes.Windsor
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Discovering;
    using Integration;
    using Integration.ContainerSetup;
    using Loading;

    public class BoxesWrapper: BoxesWrapperBase<IWindsorContainer>
    {
        private IWindsorContainer _container;
    
        public BoxesWrapper():this(new WindsorContainer()) { }

        public BoxesWrapper(IWindsorContainer container) : base(container) { }

        protected override void Initalise(IWindsorContainer container)
        {
            _container = container;
            container.Register(
                Component.For<PackageRegistry>().LifestyleSingleton(),
                Component.For<IBoxesContainerSetup>().ImplementedBy<BoxesContainerSetup>().LifestyleSingleton(),
                Component.For<IDependencyResolver>().ImplementedBy<DependencyResolver>().LifestyleSingleton(),
                Component.For<IWindsorContainer>().Instance(container).LifestyleSingleton()
                );
        }

        public override IDependencyResolver DependencyResolver { get { return _container.Resolve<IDependencyResolver>(); } }

        public override void Setup<TLoader>(IPackageScanner defaultPackageScanner)
        {
            _container.Register(
                Component.For<ILoader>().ImplementedBy<TLoader>().LifestyleSingleton(),
                Component.For<IPackageScanner>().UsingFactoryMethod(kernal => defaultPackageScanner).LifestyleSingleton()
                );
        }

        public override  void Dispose()
        {
            _container.Dispose();
        }
    }
}