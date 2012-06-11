using System.IO;
using MongoDB.Driver.GridFS;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Persistance
{
    public class MongoFileStorage<T> : MongoGatewayBase<T>
    {
        public byte[] GetFile(string id)
        {
            if (id.IsNullOrEmpty())
                return null;

            byte[] bytes = null;
            MongoGridFSFileInfo file = GridFS.FindOneById(id);

            if (file != null)
            {
                using (var stream = file.OpenRead())
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                }
            }

            return bytes;
        }

        public void PutFile(string id, string fileName, byte[] content)
        {
            using (var imageStream = new MemoryStream(content))
            {
                GridFS.Upload(imageStream, fileName, new MongoGridFSCreateOptions { Id = id });
            }
        }

        public void DeleteFile(string id)
        {
            GridFS.DeleteById(id);
        }
    }
}