using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenPinBowling.Models;

namespace TenPinBowling.Services
{
    public interface ITenPinBowlingService
    {
        IList<BowlingScoreCard> CreateUsersScorecard(IList<BowlingUser> bowlingUsers);
        IList<BowlingUser> GetBowlingUser();
        void AddScore(int userId, int frameNumber, int bowlNumber, string score);
        IList<BowlingScoreCard> GetBowlingScoreCardWithTotals();
    }
}
