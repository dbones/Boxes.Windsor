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
    using System.Linq;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Integration.Setup;
    using Integration.Setup.Interception;
    using Integration.Setup.Registrations;
    using Tasks;

    internal class RegistrationTask : IBoxesTask<RegistrationContext<IWindsorContainer>>
    {
        private readonly RegistrationMeta _registration;
        private readonly IInterceptionSelector _interceptionSelector;
        private readonly MapLifeStleToWindsor _mapLifeStleToWindsor;

        public RegistrationTask(RegistrationMeta registration, IInterceptionSelector interceptionSelector, MapLifeStleToWindsor mapLifeStleToWindsor)
        {
            _registration = registration;
            _interceptionSelector = interceptionSelector;
            _mapLifeStleToWindsor = mapLifeStleToWindsor;
        }

        public bool CanHandle(RegistrationContext<IWindsorContainer> item)
        {
            return _registration.Where(item.Type);
        }

        public void Execute(RegistrationContext<IWindsorContainer> item)
        {
            var typeToRegister = item.Type;
            var interfaces = _registration.With(typeToRegister).ToList();
            var reg = Component.For(interfaces).ImplementedBy(typeToRegister);
            
            
            var lifeStyle = (Type) _registration.LifeStyle;
            if (typeof(LifeStyle).IsAssignableFrom(lifeStyle))
            {
                lifeStyle = _mapLifeStleToWindsor.GetLifeStyle(lifeStyle);
            }
            reg.LifestyleCustom(lifeStyle);    


            if (_registration.FactoryMethod != null)
            {
                reg.UsingFactoryMethod(kernal => _registration.FactoryMethod);
            }

            var interceptorContext = new InterceptionContext() { Contracts = interfaces, Service = typeToRegister };
            reg.Interceptors(_interceptionSelector.InterceptorsToApply(interceptorContext).ToArray());

            foreach (var configuraition in _registration.Configurations)
            {
                Action<object> configuraition1 = configuraition;
                configuraition1(reg);
            }

            item.Builder.Register(reg);

        }
    }
}