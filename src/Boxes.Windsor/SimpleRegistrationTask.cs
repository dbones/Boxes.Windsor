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
    using System;
    using Castle.DynamicProxy.Internal;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Tasks;

    /// <summary>
    /// scan for items which meet the pattern and register with the IoC
    /// </summary>
    /// <typeparam name="TLifeStyle">the castle lifestyle</typeparam>
    /// <typeparam name="TInterface">the interface to register with</typeparam>
    public class SimpleRegistrationTask<TLifeStyle, TInterface> : IBoxesTask<Type>
    {
        private readonly IWindsorContainer _container;
        private readonly Type _lifeStyle;
        private readonly Type _dependencyInterface;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="container"></param>
        public SimpleRegistrationTask(IWindsorContainer container)
        {
            _container = container;
            _lifeStyle = typeof(TLifeStyle);
            _dependencyInterface = typeof(TInterface);
        }

        public bool CanHandle(Type item)
        {
            return _dependencyInterface.IsAssignableFrom(item);
        }

        public void Execute(Type item)
        {
            _container.Register(
                Component
                    .For(item.GetAllInterfaces())
                    .ImplementedBy(item)
                    .LifestyleCustom(_lifeStyle)
                );
        }
    }
}