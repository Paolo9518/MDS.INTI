using Autofac;
using Microsoft.Extensions.Configuration;
using Minedu.Comun.Data;
using MDS.Inventario.Api.DataAccess.Context;
using MDS.Inventario.Api.DataAccess.Contracts.UnitOfWork;
using MDS.Inventario.Api.DataAccess.UnitOfWork;
using System;
using System.Reflection;

namespace MDS.Inventario.Api.CrossCutting
{
    public class ContextDbModule : Autofac.Module
    {

        public static IConfiguration Configuration;

        protected override void Load(ContainerBuilder builder)
        {
            string connectionString = Configuration.GetSection("ConnectionStrings:MDSInventarioDBContext").Value;

            //Context           
            builder.RegisterType<CertificadoDbContext>().Named<IDbContext>("context").WithParameter("connstr", connectionString).InstancePerLifetimeScope();
            //Resolver UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<IDbContext>("context"));

            //-> Aplicacion
            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("MDS.Inventario.Api.Application")))
                .Where(t => t.Name.EndsWith("Service", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("MDS.Inventario.Api.Application")))
                .Where(t => t.Name.EndsWith("Config", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

            /*builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("MDS.Inventario.Api.Application")))
                .Where(t => t.Name.EndsWith("Caller", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();*/


            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("MDS.Inventario.Api.Application")))
                .Where(t => t.Name.EndsWith("Security", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();
        }
    }
}
