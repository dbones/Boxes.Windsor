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
    using System.Collections.Generic;
    using Castle.Windsor;
    using Integration;

    public class DependencyResolver : IDependencyResolver
    {
        internal IWindsorContainer Container { get; private set; }

        public DependencyResolver(IWindsorContainer container)
        {
            Container = container;
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return Container.Resolve(type);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return Container.ResolveAll<T>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            var enumerator = Container.ResolveAll(type).GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public void Release(object obj)
        {
            Container.Release(obj);
        }
    }
}
