namespace Boxes.Windsor
{
    using Castle.Windsor;
    using Integration.Setup;
    using Tasks;

    internal class RegistrationTaskMapper : IRegistrationTaskMapper<IWindsorContainer>
    {
        public IBoxesTask<RegistrationContext<IWindsorContainer>> CreateRegisterTask(RegistrationMeta registration)
        {
            return new RegistrationTask(registration);
        }
    }
}