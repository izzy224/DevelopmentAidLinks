using Autofac;
using LinkExtractor.DAL;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.ViewModel;

namespace LinkExtractor.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<LinkExtractorDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<EmployeeDataService>().As<IEmployeeDataService>();

            return builder.Build();
        }
    }
}
