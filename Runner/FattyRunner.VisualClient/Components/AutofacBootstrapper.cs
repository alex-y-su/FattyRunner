using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Azon.Helpers.Extensions;

using Caliburn.Micro;
using Autofac;

using Caliburn.Micro.Logging;

using FattyRunner.VisualClient.ViewModel;

namespace FattyRunner.VisualClient.Components {
    public class AutofacBootstrapper : Bootstrapper<ShellViewModel> {
        protected IContainer Container { get; private set; }

        public AutofacBootstrapper() {
            LogManager.GetLog = t => new TraceLogger(t);
        }

        protected override void Configure() {
            base.Configure();

            var builder = new ContainerBuilder();

            this.ConfigureContainer(builder);
            this.Container = builder.Build();
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("ViewModel"))
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
                   .Where(type => type.Name.EndsWith("View"))
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<WindowManager>()
                   .As<IWindowManager>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<EventAggregator>()
                   .As<IEventAggregator>()
                   .InstancePerLifetimeScope();
        }

        protected override void BuildUp(object instance) {
            this.Container.InjectProperties(instance);
        }

        protected override object GetInstance(Type service, string key) {
            return key.IsNullOrWhiteSpace()
                ? this.Container.Resolve(service)
                : this.Container.ResolveNamed(key, service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return this.Container.Resolve(
                typeof(IEnumerable<>).MakeGenericType(service)
            ) as IEnumerable<object>;
        }

        protected virtual object GetInstanceUsingParameters(Type service, object[] parameters) {
            var @params = parameters.Select(p => new TypedParameter(p.GetType(), p));
            return this.Container.Resolve(service, @params);
        }
    }
}
