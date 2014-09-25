using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenPinBowling.Models;

namespace TenPinBowling.Repositories
{
    /// <summary>
    /// TODO use generics, can use a common IRepository by passing type
    /// </summary>
    public interface IBowlingCardRepository
    {
        void Setup();
        IList<BowlingScoreCard> Fetch();
        IList<BowlingScoreCard> Save(IList<BowlingScoreCard> bowlingScorecard);
    }
}
