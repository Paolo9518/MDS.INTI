using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDS.Inventario.Api.CrossCutting
{
    public static class BootstrapperContainer
    {
        public static IConfiguration Configuration;

        public static void Register(ContainerBuilder builder)
        {
            //Add Context
            ContextDbModule.Configuration = Configuration;
            builder.RegisterModule<ContextDbModule>();
        }
    }
}
