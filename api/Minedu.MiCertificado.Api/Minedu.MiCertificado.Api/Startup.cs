using Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Minedu.MiCertificado.Api.CrossCutting;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Antiforgery;
using System.Collections.Generic;

namespace Minedu.MiCertificado.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();

            //Opciones antiforgery
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "AntiforgeryFieldname";
                //options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    //options.RequireHttpsMetadata = false;
                    //options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("JwT:Issuer").Value,
                        ValidAudience = Configuration.GetSection("JwT:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwT:Key").Value)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "Mi-Certificado API",
                        Description = "A simple example ASP.NET Core Web API",
                        TermsOfService = "https://example.com/terms",
                        Contact = new Contact
                        {
                            Name = "Edwx",
                            Email = string.Empty,
                            Url = "https://twitter.com/spboyer",
                        },
                        License = new License
                        {
                            Name = "Use under LICX",
                            Url = "https://example.com/license",
                        }
                    });
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                });

            var valuesSection = Configuration
                .GetSection("Cors:AllowedHost")
                //.Value;
                .Get<List<string>>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(valuesSection.ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => 
                {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //quitar headers
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    //PARA ELIMINNAR ENCABEZADO SERVER
                    context.Response.Headers.Remove("Server");
                    context.Response.Headers.Add("Server", "ANONYM");

                    //IMPOSIBLE MOSTRAR sitio web dentro de un iframe
                    context.Response.Headers.Remove("X-Frame-Options");
                    context.Response.Headers.Add("X-Frame-Options", "DENY");

                    //IMPOSIBLE inyectar JavaScript en un archivo svg.
                    context.Response.Headers.Remove("X-Content-Type-Options");
                    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                    //Detener el ataque XSS. Este tipo de encabezado es útil principalmente para navegadores antiguos.
                    context.Response.Headers.Remove("X-Xss-Protection");
                    context.Response.Headers.Add("X-Xss-Protection", "1");
                    
                    return Task.FromResult(0);
                });
                await next();
            });

            //ENVIAR TOKEN ANTIFORGERY
            app.Use(next => context =>
            {
                // El token de solicitud se puede enviar como una cookie legible para JavaScript,
                // y Angular lo usa por defecto.
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                    new CookieOptions() { HttpOnly = false });

                return next(context);
            });

            app.UseHttpsRedirection();

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Register Types
            BootstrapperContainer.Configuration = Configuration;
            BootstrapperContainer.Register(builder);
        }
    }
}
