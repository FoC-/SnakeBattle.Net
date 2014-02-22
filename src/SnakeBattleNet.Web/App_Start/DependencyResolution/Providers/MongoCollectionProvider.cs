using System;
using System.Collections.Generic;
using MongoDB.Driver;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.Utils;
using StructureMap;

namespace SnakeBattleNet.Web.DependencyResolution.Providers
{
    public class MongoCollectionProvider
    {
        private readonly MongoDatabase _database;
        private static readonly Dictionary<Type, Action<object>> IndexEnsurers = new Dictionary<Type, Action<object>>
        {
            {typeof (MongoCollection<UserIdentity>), EnsureUserIdentity},
            {typeof (MongoCollection<Snake>), EnsureSnake}
        };

        public MongoCollectionProvider(MongoDatabase database)
        {
            _database = database;
        }

        public object ProvideCollection(IContext c)
        {
            var requestedType = c.BuildStack.Current.RequestedType;
            var type = requestedType.GetGenericArguments()[0];
            var collection = _database.GetCollection(type, type.Name);

            Action<object> ensure;
            if (IndexEnsurers.TryGetValue(collection.GetType(), out ensure)) ensure(collection);

            return collection;
        }

        private static void EnsureUserIdentity(object collection)
        {
            var userIdentity = (MongoCollection<UserIdentity>)collection;
            userIdentity.EnsureIndex(Util.GetElementNameFor<UserIdentity>(_ => _.UserName));
        }

        private static void EnsureSnake(object collection)
        {
            var snakesCollection = (MongoCollection<Snake>)collection;
            snakesCollection.EnsureIndex(Util.GetElementNameFor<Snake>(_ => _.Id));
            snakesCollection.EnsureIndex(Util.GetElementNameFor<Snake>(_ => _.OwnerId));
        }
    }
}