using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TenPinBowling.Models;

namespace TenPinBowling.Repositories
{
    public class BowlingUserRepository: IBowlingUserRepository
    {

        public IList<BowlingUser> Fetch()
        {
            return System.Web.HttpContext.Current.Session["BowlingUsers"] as IList<BowlingUser>;
        }

        public IList<BowlingUser> Save(IList<BowlingUser> bowlingusers)
        {
            System.Web.HttpContext.Current.Session["BowlingUsers"] = bowlingusers;
            return System.Web.HttpContext.Current.Session["BowlingUsers"] as IList<BowlingUser>;
        }

    }
}