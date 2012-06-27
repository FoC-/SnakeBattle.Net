using System.ComponentModel.DataAnnotations;

namespace SnakeBattleNet.Web.Models
{
    public class SnakeNameViewModel : SnakeViewModelBase
    {
        [Display(Name = "Snake name")]
        [StringLength(20, ErrorMessage = "Maximum length for name is 20 symbols")]
        public string Name { get; set; }

        public SnakeNameViewModel() { }
        public SnakeNameViewModel(string id) : base(id) { }
        public SnakeNameViewModel(string id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}