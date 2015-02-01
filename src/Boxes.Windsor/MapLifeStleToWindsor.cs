namespace Boxes.Windsor
{
    using System;
    using System.Collections.Generic;
    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;
    using Integration.Extensions;
    using Integration.Setup.Registrations;

    public class MapLifeStleToWindsor : IBoxesExtensionWithSetup
    {
        private readonly IDictionary<Type, Type> mappers = new Dictionary<Type, Type>();

        public MapLifeStleToWindsor()
        {
            //do the default ones
            Add<Singleton, SingletonLifestyleManager>();
            Add<Transient, TransientLifestyleManager>();
        }


        public void Add<TSrc, TDest>() 
            where TSrc : LifeStyle
            where TDest : ILifestyleManager
        {
            mappers.Add(typeof(TSrc), typeof(TDest));
        }

        internal Type GetLifeStyle(Type lifeStyle)
        {
            return mappers[lifeStyle];
        }

    }
}