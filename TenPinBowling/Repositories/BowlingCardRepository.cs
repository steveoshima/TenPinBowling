using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using TenPinBowling.Models;

namespace TenPinBowling.Repositories
{
    public class BowlingCardRepository : IBowlingCardRepository
    {

        public void Setup()
        {
            //no direct db so storing in Session, would never do this if at all poss in mvc.net
            System.Web.HttpContext.Current.Session["BowlingCards"] = new List<BowlingScoreCard>();
        } 

        public IList<BowlingScoreCard> Fetch()
        {
            return System.Web.HttpContext.Current.Session["BowlingCards"] as IList<BowlingScoreCard>;
        }

        public IList<BowlingScoreCard> Save(IList<BowlingScoreCard> bowlingScorecard)
        {
            System.Web.HttpContext.Current.Session["BowlingCards"] = bowlingScorecard;
            return System.Web.HttpContext.Current.Session["BowlingCards"] as IList<BowlingScoreCard>;
        }

    }
}