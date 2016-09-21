using System;
using Autofac;
using Downloader.Services;
using Downloader.ViewModels;

namespace Downloader.Infrastructure
{
    public class Factory
    {
        private static readonly Lazy<IContainer> Container = new Lazy<IContainer>(Init);

        private static IContainer Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DownloadViewModel>().AsSelf();
            builder.RegisterType<PageLoader>().AsImplementedInterfaces();
            builder.RegisterType<PageSaver>().AsImplementedInterfaces();
            builder.RegisterType<ValidationService>().AsImplementedInterfaces();

            var container = builder.Build();
            return container;
        }

        public static T Resolve<T>()
        {
            return Container.Value.Resolve<T>();
        }
    }
}
