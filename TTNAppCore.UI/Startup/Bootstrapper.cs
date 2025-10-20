using Autofac;
using TTNAppCode.UI;
using TTNAppCore.DataAccess;
using TTNAppCore.UI.Data;
using TTNAppCore.UI.Data.Lookups;
using TTNAppCore.UI.Data.Repositories;
using TTNAppCore.UI.Export;
using TTNAppCore.UI.View.Services;
using TTNAppCore.UI.ViewModel;

namespace TTNAppCore.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<TTNDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<TtnXlsxExporter>().As<ITtnXlsxExporter>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<TtnDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(TtnDetailViewModel));
            builder.RegisterType<DriverDetailViewModel>()
                .Keyed<IDetailViewModel>(nameof(DriverDetailViewModel));

            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<TtnRepository>().As<ITtnRepository>();
            builder.RegisterType<DriverRepository>().As<IDriverRepository>();

            return builder.Build();
        }
    }
}
