﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json.Serialization;
using Owin;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Web;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.DependencyResolution;
using SnakeBattleNet.Web.DependencyResolution.Providers;
using SnakeBattleNet.Web.Models.Snake;
using StructureMap;

[assembly: OwinStartup(typeof(Bootstrap), "RegisterOwin")]
[assembly: WebActivator.PreApplicationStartMethod(typeof(Bootstrap), "RegisterDependencies")]

namespace SnakeBattleNet.Web
{
    public class Bootstrap
    {
        public static void RegisterOwin(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"),
                Provider = new CookieAuthenticationProvider()
            });
        }

        public static void RegisterApis(HttpConfiguration config)
        {
            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/kinetic").Include(
                "~/Scripts/kinetic-v5.0.1.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/site.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));
        }

        public static void RegisterDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.For<IUserStore<UserIdentity>>().Use<CustomUserStore<UserIdentity>>();
                x.For<IUserRoleStore<UserIdentity>>().Use<CustomUserStore<UserIdentity>>();
                x.For<IUserSearch<UserIdentity>>().Use<CustomUserStore<UserIdentity>>();
                x.For<IAuthenticationManager>().HttpContextScoped().Use(() => HttpContext.Current.Request.GetOwinContext().Authentication);

                // Mongo
                x.For<MongoDatabaseProvider>().Use<MongoDatabaseProvider>()
                    .Ctor<string>().Is(ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle");

                x.For<MongoDatabase>().Singleton().Use(c => c.GetInstance<MongoDatabaseProvider>().ProvideDatabase());
                x.For(typeof(MongoCollection<>)).Use(c => c.GetInstance<MongoCollectionProvider>().ProvideCollection(c));
                x.For<ISnakeStore>().Singleton().Use<SnakeStore>();
                x.For<IFileStore>().Singleton().Use<FileStore>()
                    .Ctor<MongoGridFS>().Is(c => c.GetInstance<MongoDatabase>().GridFS);
            });

            var container = ObjectFactory.Container;
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
        }

        public static void RegisterMappings()
        {
            Mapper.CreateMap<IDictionary<Position, ChipCell>, IEnumerable<ChipCellViewModel>>()
                .ConvertUsing(v => v.Select(p => new ChipCellViewModel
                {
                    P = p.Key,
                    C = p.Value.Content,
                    Color = p.Value.Color,
                    Exclude = p.Value.Exclude,
                    IsSelf = p.Value.IsSelf
                }));
            Mapper.CreateMap<IEnumerable<ChipCellViewModel>, IDictionary<Position, ChipCell>>()
                .ConvertUsing(v => v.ToDictionary(k => k.P, va => new ChipCell
                {
                    Color = va.Color,
                    Content = va.C,
                    Exclude = va.Exclude,
                    IsSelf = va.IsSelf
                }));

            Mapper.CreateMap<Replay, ReplayViewModel>();
            Mapper.CreateMap<IDictionary<Position, Content>, IEnumerable<ContentViewModel>>()
                .ConvertUsing(v => v.Select(p => new ContentViewModel { P = p.Key, C = p.Value }));

            Mapper.CreateMap<Position, Position>();
            Mapper.CreateMap<Snake, SnakeViewModel>();

            Mapper.CreateMap<Tuple<Snake, SnakeViewModel>, Snake>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Item2.Name))
                .ForMember(d => d.Chips, o => o.MapFrom(s => s.Item2.Chips))

                .ForMember(d => d.Id, o => o.MapFrom(s => s.Item1.Id))
                .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.Item1.OwnerId))
                .ForMember(d => d.Created, o => o.MapFrom(s => s.Item1.Created))
                .ForMember(d => d.Score, o => o.MapFrom(s => s.Item1.Score))
                .ForMember(d => d.Wins, o => o.MapFrom(s => s.Item1.Wins))
                .ForMember(d => d.Loses, o => o.MapFrom(s => s.Item1.Loses))
                .ForMember(d => d.Matches, o => o.MapFrom(s => s.Item1.Matches));
        }
    }
}
