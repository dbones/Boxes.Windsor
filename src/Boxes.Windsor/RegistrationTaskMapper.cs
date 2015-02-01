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
    using Integration.Setup;
    using Tasks;

    internal class RegistrationTaskMapper : IRegistrationTaskMapper<IWindsorContainer>
    {
        private readonly MapLifeStleToWindsor _mapLifeStleToWindsor;
        private IInterceptionSelector _interceptionSelector;

        public RegistrationTaskMapper(MapLifeStleToWindsor mapLifeStleToWindsor)
        {
            _mapLifeStleToWindsor = mapLifeStleToWindsor;
        }

        public void SetInterceptionSelector(IInterceptionSelector interceptionSelector)
        {
            _interceptionSelector = interceptionSelector;
        }

        public IBoxesTask<RegistrationContext<IWindsorContainer>> CreateRegisterTask(RegistrationMeta registration)
        {
            return new RegistrationTask(registration, _interceptionSelector, _mapLifeStleToWindsor);
        }
    }
}