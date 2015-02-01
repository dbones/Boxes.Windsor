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
    using System.Collections.Concurrent;
    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;
    using Integration;
    using Integration.Factories;
    using Integration.InternalIoc;
    using Integration.Setup;


    public class BoxesWrapper: BoxesWrapperBase<IWindsorContainer, IWindsorContainer>
    {
        protected override void Initialize(IInternalContainer internalContainer)
        {
            internalContainer.Add(typeof(IIocFactory<,>), typeof(IocFactory)); //hmmm, do not like that much
            internalContainer.Add(typeof(IRegistrationTaskMapper<>), typeof(RegistrationTaskMapper));
            internalContainer.Add<IDependencyResolverFactory, DependencyResolverFactory>();
            internalContainer.Add<MapLifeStleToWindsor, MapLifeStleToWindsor>();
        }
    }
}