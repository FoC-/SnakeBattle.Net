using System.Collections.Generic;
using System.Linq;

namespace SnakeBattleNet.Web.Models.Snake
{
    public class ChipViewModel
    {
        public IEnumerable<CellViewModel> Cells { get; set; }

        public ChipViewModel()
        {
            Cells = Enumerable.Empty<CellViewModel>();
        }
    }
}