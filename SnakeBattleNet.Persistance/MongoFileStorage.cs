using System.IO;
using MongoDB.Driver.GridFS;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Persistance
{
    public class MongoFileStorage<T> : MongoGatewayBase<T>
    {
        public static Stream GetFile(string id)
        {
            if (id.IsNullOrEmpty())
                return null;

            MongoGridFSFileInfo file = GridFS.FindOneById(id);
            return file == null ? null : file.OpenRead();
        }

        public static void PutFile(string id, string fileName, Stream content, string contentType)
        {
            GridFS.Upload(content, fileName, new MongoGridFSCreateOptions { Id = id, ContentType = contentType });
        }

        public static void DeleteFile(string id)
        {
            GridFS.DeleteById(id);
        }
    }
}