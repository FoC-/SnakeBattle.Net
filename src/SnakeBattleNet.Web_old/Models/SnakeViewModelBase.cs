using System.ComponentModel.DataAnnotations;

namespace SnakeBattleNet.Web.Models
{
    public class SnakeViewModelBase
    {
        public string Id { get; set; }

        [Display(Name = "Snake name")]
        [StringLength(20, ErrorMessage = "Maximum length for name is 20 symbols")]
        public string Name { get; set; }

        public SnakeViewModelBase() { }
        public SnakeViewModelBase(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}