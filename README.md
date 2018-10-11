Aspnet.Identity.Akka

- open for Pull Requests
- ad hoc tested using the provided Profile Web project

no guarantees, this repository is provided as is, to demonstrate how on identity store could be made with Akka as data provider. Still a lot of work in progress. Need to move away from the helpers, removing the receiveactor. Using configuration to call an external persister (as in the current example), or use the regular Persister. No need for 2 actor types.

Added the google authentication from the example found on http://docs.identityserver.io/en/release/quickstarts/4_external_authentication.html

Example ConfigureServices creating a fast and simple first setup (with a forget always actor system)
```
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

            services.AddIdentityCore<ApplicationUser>()
                .AddUserStore()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies(_ => { });

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
```
