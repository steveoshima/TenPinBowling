using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TenPinBowling.Models
{
    public class BowlingFrame
    {
        public string FirstBowl { get; set; }
        public string SecondBowl { get; set; }
        public string FinalBowl { get; set; } // may think about moving this
        public int Total { get; set; }
    }
}