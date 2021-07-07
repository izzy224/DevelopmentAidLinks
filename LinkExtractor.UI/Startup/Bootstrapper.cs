using Autofac;
using LinkExtractor.DAL;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.ViewModel;
using Prism.Events;

namespace LinkExtractor.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<LinkExtractorDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<EmployeeDetailViewModel>().As<IEmployeeDetailViewModel>();

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<EmployeeDataService>().As<IEmployeeDataService>();
            

            return builder.Build();
        }
    }
}
