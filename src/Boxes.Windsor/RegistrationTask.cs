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
    using System;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Integration.Setup;
    using Tasks;

    internal class RegistrationTask : IBoxesTask<RegistrationContext<IWindsorContainer>>
    {
        private readonly RegistrationMeta _registration;

        public RegistrationTask( RegistrationMeta registration)
        {
            _registration = registration;
        }
        
        public bool CanHandle(RegistrationContext<IWindsorContainer> item)
        {
            return _registration.Where(item.Type);
        }

        public void Execute(RegistrationContext<IWindsorContainer> item)
        {
            var typeToRegister = item.Type;
            var interfaces = _registration.With(typeToRegister);
            var reg = Component.For(interfaces).ImplementedBy(typeToRegister).LifestyleCustom((Type)_registration.LifeStyle);

            if (_registration.FactoryMethod != null)
            {
                reg.UsingFactoryMethod(kernal => _registration.FactoryMethod);
            }

            foreach (var configuraition in _registration.Configurations)
            {
                Action<object> configuraition1 = configuraition;
                configuraition1(reg);
            }

            item.Builder.Register(reg);

        }
    }
}