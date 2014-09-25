using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TenPinBowling.Models
{
    public class BowlingTurn
    {
        public int UserId { get; set; }
        public int FrameNumber { get; set; } 
        public int BowlNumber { get; set; }
        public string Score { get; set; }
    }
}