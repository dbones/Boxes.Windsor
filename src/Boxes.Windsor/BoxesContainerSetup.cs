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
    using Castle.Windsor;
    using Integration.ContainerSetup;

    public class BoxesContainerSetup : BoxesContainerSetupBase
    {
        
        private readonly IWindsorContainer _container;
        
        public BoxesContainerSetup(IWindsorContainer container)
        {
            _container = container;
        }

        public override void RegisterLifeStyle<TLifeStyle, TInterface>()
        {
            AddRegistrationTask(new SimpleRegistrationTask<TLifeStyle, TInterface>(_container));
        }

        protected override void RegisterLifeStyle(RegistrationMeta registration)
        {
            AddRegistrationTask(new AdvancedRegistrationTask(_container, registration));
        }
    }
}