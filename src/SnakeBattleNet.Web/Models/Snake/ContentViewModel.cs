using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnakeBattleNet.Core.Contract;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ContentViewModel
    {
        public Position P { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Content C { get; set; }
    }
}