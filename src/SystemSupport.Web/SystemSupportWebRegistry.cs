using SystemSupport.Core;
using SystemSupport.Web.Areas.Permissions.Controllers;
using SystemSupport.Web.Areas.Permissions.Grids;
using SystemSupport.Web.Grids;
using SystemSupport.Web.Menus;
using SystemSupport.Web.Services;


namespace SystemSupport.Web
{
    using SystemSupport.Web.Services.ViewOptions;

    using Alpinely.TownCrier;

    using CC.Core.Domain;
    using CC.Core.DomainTools;
    using CC.Core.Html.CCUI.HtmlConventionRegistries;
    using CC.Core.Html.Grid;
    using CC.Core.Localization;
    using CC.Core.Services;
    using CC.Security;
    using CC.Security.Interfaces;
    using CC.Security.Services;
    using CC.UI.Helpers;
    using CC.UI.Helpers.Configuration;
    using CC.UI.Helpers.Tags;

    using KnowYourTurf.Core;
    using KnowYourTurf.Core.Config;
    using KnowYourTurf.Core.Domain;
    using KnowYourTurf.Core.Domain.Persistence;
    using KnowYourTurf.Core.Html.HtmlConventionRegistries;
    using KnowYourTurf.Core.Services;

    using Microsoft.Practices.ServiceLocation;

    using NHibernate;

    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    using Log4NetLogger = KnowYourTurf.Core.Log4NetLogger;
    using StructureMapServiceLocator = SystemSupport.Web.Config.StructureMapServiceLocator;

    public class SystemSupportWebRegistry : Registry
    {
        public SystemSupportWebRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.ConnectImplementationsToTypesClosing(typeof(IEntityListGrid<>));
                x.AssemblyContainingType(typeof(CoreLocalizationKeys));
                x.AssemblyContainingType(typeof(MergedEmailFactory));
                x.AssemblyContainingType<Entity>();
                x.AssemblyContainingType<IUser>();
                x.AssemblyContainingType<HtmlConventionRegistry>();
//                x.AddAllTypesOf<ICalculatorHandler>().NameBy(t => t.Name);
                x.AddAllTypesOf<RulesEngineBase>().NameBy(t => t.Name);
//                x.AddAllTypesOf<IEmailTemplateHandler>().NameBy(t => t.Name);
                x.WithDefaultConventions();
            });

            For<HtmlConventionRegistry>().Add<KYTKOHtmlConventionRegistry>();
            For<IServiceLocator>().Singleton().Use(new StructureMapServiceLocator());
            For<IElementNamingConvention>().Use<CCElementNamingConvention>();
            For(typeof(ITagGenerator<>)).Use(typeof(TagGenerator<>));
            For<TagProfileLibrary>().Singleton();
            For<INHSetupConfig>().Use<KYTNHSetupConfig>();

            For<ISessionFactoryConfiguration>().Singleton()
                .Use<SqlServerSessionSourceConfiguration>()
                .Ctor<SqlServerSessionSourceConfiguration>("connectionStr")
                .EqualToAppSetting("KnowYourTurf.sql_server_connection_string");
            For<ISessionFactory>().Singleton().Use(ctx => ctx.GetInstance<ISessionFactoryConfiguration>().CreateSessionFactory());

            For<ISession>().HybridHttpOrThreadLocalScoped().Use(context => context.GetInstance<ISessionFactory>().OpenSession(new SaveUpdateInterceptor()));

            For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<SysUnitOfWork>();

            For<IRepository>().Use<Repository>();

            For<ITemplateParser>().Use<TemplateParser>();

            For<ISessionContext>().Use<SSSessionContext>();
            For<ILocalizationDataProvider>().Use<LocalizationDataProvider>();
            For<IAuthenticationContext>().Use<WebAuthenticationContext>();

            For<IMenuConfig>().Use<SystemSupportMenu>();

            For<IAuthorizationService>().HybridHttpOrThreadLocalScoped().Use<AuthorizationService>();
            For<IAuthorizationRepository>().HybridHttpOrThreadLocalScoped().Use<CustomAuthorizationRepository>();
            For<IPermissionsBuilderService>().HybridHttpOrThreadLocalScoped().Use<PermissionsBuilderService>();
            For<IPermissionsService>().HybridHttpOrThreadLocalScoped().Use<PermissionsService>();
            For<ILogger>().Use(() => new Log4NetLogger(typeof(string)));
            For<IRouteTokenConfig>().Add<SystemSupportRouteTokenList>();

            For<ICCSessionContext>().Use<SessionContext>();

            For(typeof(IGridBuilder<>)).Use(typeof(GridBuilder<>));

            For<IEntityListGrid<User>>().Use<UserListGrid>();
            For<IEntityListGrid<PermissionDto>>().Use<UserPermissionListGrid>();
            For<IEntityListGrid<PermissionDto>>().Add<GroupPermissionListGrid>().Named("group");


        }
    }
}
