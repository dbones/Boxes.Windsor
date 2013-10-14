// Copyright 2012 - 2013 dbones.co.uk
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
    using Castle.Windsor;
    using Integration.Factories;

    internal class IocFactory : IIocFactory<IWindsorContainer, IWindsorContainer>
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