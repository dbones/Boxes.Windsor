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
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Integration.ContainerSetup;
    using Tasks;

    public class AdvancedRegistrationTask: IBoxesTask<Type> 
    {
        private readonly IWindsorContainer _container;
        private readonly RegistrationMeta _registration;

        public AdvancedRegistrationTask(IWindsorContainer container, RegistrationMeta registration)
        {
            _container = container;
            _registration = registration;
        }

        public bool CanHandle(Type item)
        {
            return _registration.Where(item);
        }

        public void Execute(Type item)
        {
            var interfaces =_registration.With(item);
            var reg = Component.For(interfaces).ImplementedBy(item).LifestyleCustom(_registration.LifeStyle);

            if (_registration.FactoryMethod != null)
            {
                reg.UsingFactoryMethod(kernal => _registration.FactoryMethod);
            }

            foreach (var configuraition in _registration.Configuraitions)
            {
                Action<object> configuraition1 = configuraition;
                configuraition1(reg);
            }
            _container.Register(reg);
        }
    }
}