namespace Boxes.Windsor
{
    using System;
    using Castle.MicroKernel.Registration;
    using Integration.Setup.Registrations;

    /// <summary>
    /// register and configure types with the underlying IoC container using this pattern
    /// </summary>
    public class Register : RegisterBase<Type, IRegistration>{}
}