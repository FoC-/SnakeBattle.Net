using System.IO;
using MongoDB.Driver.GridFS;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Core
{
    public class FileStore : IFileStore
    {
        private readonly MongoGridFS _gridFs;

        public FileStore(MongoGridFS mongoGridFs)
        {
            _gridFs = mongoGridFs;
        }

        public Stream ReadFile(string id, out string contentType)
        {
            contentType = string.Empty;

            if (id.IsNullOrEmpty())
                return null;

            MongoGridFSFileInfo file = _gridFs.FindOneById(id);
            if (file == null)
                return null;

            contentType = file.ContentType;
            return file.OpenRead();
        }

        public void SaveFile(string id, string fileName, Stream content, string contentType)
        {
            MongoGridFSFileInfo file = _gridFs.FindOneById(id);
            if (file != null)
                DeleteFile(id);

            _gridFs.Upload(content, fileName, new MongoGridFSCreateOptions { Id = id, ContentType = contentType });
        }

        public void DeleteFile(string id)
        {
            _gridFs.DeleteById(id);
        }
    }
}