using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TenPinBowling.Models
{
    public class BowlingScoreCard
    {
        public int UserId { get; set; }
        public IList<BowlingFrame> Frames { get; set; }
        public int Order { get; set; }
    }
}