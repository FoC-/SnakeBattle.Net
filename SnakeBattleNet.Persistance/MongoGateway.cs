using System.Collections.Generic;

namespace SnakeBattleNet.Persistance
{
    public class MongoGateway<T> : MongoGatewayBase<T>
    {
        public T Get(string id)
        {
            return Collection.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Collection.FindAll();
        }

        public void Add(T o)
        {
            Collection.Insert(o);
        }

        public void Update(T o)
        {
            Collection.Save(o);
        }
    }
}
