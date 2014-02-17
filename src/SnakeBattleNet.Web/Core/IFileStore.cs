using System.IO;

namespace SnakeBattleNet.Web.Core
{
    public interface IFileStore
    {
        Stream ReadFile(string id, out string contentType);
        void SaveFile(string id, string fileName, Stream content, string contentType);
        void DeleteFile(string id);
    }
}