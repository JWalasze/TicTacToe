using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Models
{
    public record Game
    {
        public int Id { get; set; }

        public Player Player1 { get; set; }

        public Player Player2 { get; set; }

        public DateTime Time { get; set; }

        public Player Winner { get; set; }

        public bool IsDrawback { get; set; }
    }
}
