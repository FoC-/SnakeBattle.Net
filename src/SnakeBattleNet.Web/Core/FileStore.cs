using System.IO;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Core
{
    public class FileStore : IFileStore
    {
        private readonly MongoDatabase _dataBase;

        private MongoGridFS GridFS
        {
            get { return _dataBase.GridFS; }
        }

        public FileStore(MongoDatabase dataBase)
        {
            _dataBase = dataBase;
        }

        public Stream ReadFile(string id, out string contentType)
        {
            contentType = string.Empty;

            if (id.IsNullOrEmpty())
                return null;

            MongoGridFSFileInfo file = GridFS.FindOneById(id);
            if (file == null)
                return null;

            contentType = file.ContentType;
            return file.OpenRead();
        }

        public void SaveFile(string id, string fileName, Stream content, string contentType)
        {
            MongoGridFSFileInfo file = GridFS.FindOneById(id);
            if (file != null)
                DeleteFile(id);

            GridFS.Upload(content, fileName, new MongoGridFSCreateOptions { Id = id, ContentType = contentType });
        }

        public void DeleteFile(string id)
        {
            GridFS.DeleteById(id);
        }
    }
}