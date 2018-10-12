using Finbuckle.MultiTenant.Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Models;
using Profile.Services;
using Profile.Tenant;
using System;
using System.Collections.Generic;
using Aspnet.Identity.Akka.ExtensionMethods;
using Aspnet.Identity.Akka;
using Aspnet.Identity.Akka.Actors;
using Akka.Actor;
using Akka.Configuration;
using System.IO;

namespace Profile
{

    //later: https://github.com/Dresel/RouteLocalization/blob/master/Documentation/GettingStarted.md
    //routes vertalen
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var fn = Configuration.GetValue<string>("hoconfile");
            var config = ConfigurationFactory.ParseString(File.ReadAllText(fn));
            services.AddSingleton(new SimplePersister());
            services.AddSingleton(_ => ActorSystem.Create(config.GetString("akka.systemname"), config));
            services.AddSingleton(sp => {
                var system = sp.GetService<ActorSystem>();
                var persister = sp.GetService<SimplePersister>();
                IActorRef actorRef = system.ActorOf(Props.Create(() => new UserCoordinator<Guid, ApplicationUser>(true, persister.CoordinatorPerist, persister.UserPersist)));
                return new ActorRefFor<UserCoordinator<Guid, ApplicationUser>>(actorRef);
            });

            services.ConfigurePOCO<List<TenantConfiguration>>(Configuration.GetSection("TenantConfigurations"));
            services.Configure<RazorViewEngineOptions>(options => options.ViewLocationExpanders.Add(new TenantViewLocationExpander()));

            services.AddCalqoDefaultIdentity<ApplicationUser>(o =>
                {
                    o.User.RequireUniqueEmail = true;
                    o.SignIn.RequireConfirmedEmail = true;
                })
                .AddUserStore();

            services.AddSingleton<IMultiTenantStore>((serviceProvider) => new TenantResolver(serviceProvider.GetService<List<TenantConfiguration>>()));
            services.AddMultiTenant()
                .WithStrategy<TenantStrategy>()
                .AddPerTenantSocialLogins();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>(); // start Akka.NET
                app.ApplicationServices.GetService<ActorRefFor<UserCoordinator<Guid, ApplicationUser>>>();
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait();
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMultiTenant(ConfigRoutes);
            app.UseAuthentication();
            app.UseMvc(ConfigRoutes);
        }

        private void ConfigRoutes(IRouteBuilder routes)
        {
            routes.MapRoute("Default", "{controller=Manage}/{action=Index}");
        }
    }
}
