using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Common.Repository;
using Cookbook.Repository;
using System.Data.Entity;

namespace Cookbook.Castle
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store) => container
                .Register(Component.For(new[] { typeof(IUnitOfWork), typeof(CookbookDbContext), typeof(DbContext) })
                                   .ImplementedBy<CookbookDbContext>().LifestyleScoped(),
                          Component.For(typeof(IDataSet<>)).ImplementedBy(typeof(DataSet<>)).LifestyleScoped(),
                          Component.For<IDataSetFactory>().AsFactory().LifestyleSingleton());
    }
}
