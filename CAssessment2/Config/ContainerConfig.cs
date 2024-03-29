﻿namespace CAssessment2.Config
{
    using System.Linq;
    using SimpleInjector;

    public static class ContainerConfig
    {
        private static Container Container;

        public static void Init()
        {
            Container = new Container();

            RegisterAllTypesWithConvention();
            //RegisterLoggerFactory();

            Container.Verify();
        }

        public static TService GetInstance<TService>() where TService : class
        {
            return Container.GetInstance<TService>();
        }

        private static void RegisterAllTypesWithConvention()
        {
            var typesWithInterfaces = typeof(Program).Assembly.GetExportedTypes()
                .Where(t => t.Namespace.StartsWith("CAssessment2"))
                .Where(ts => ts.GetInterfaces().Any() && ts.IsClass).ToList();
            var registrations = typesWithInterfaces
                .Where(t => t.GetInterfaces().Length == 1)
                .Select(ti => new {Service = ti.GetInterfaces().Single(), Implementation = ti})
                .Where(r => r.Service.Name == "I" + r.Implementation.Name);

            foreach (var reg in registrations)
            {
                Container.Register(reg.Service, reg.Implementation, Lifestyle.Singleton);
            }
        }

        //private static void RegisterLoggerFactory()
        //{
        //    var loggerFactory = new Microsoft.Extensions.Logging.LoggerFactory();
        //    loggerFactory.AddLog4Net(true);
        //    Container.RegisterInstance< Microsoft.Extensions.Logging.ILoggerFactory>(loggerFactory);
        //}
    }
}
