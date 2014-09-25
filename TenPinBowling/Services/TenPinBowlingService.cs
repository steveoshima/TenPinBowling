using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TenPinBowling.Models;
using TenPinBowling.Repositories;

namespace TenPinBowling.Services
{
    /// <summary>
    /// </summary>
    public class TenPinBowlingService : ITenPinBowlingService
    {
        private IBowlingCardRepository _bowlingCardRepository;
        private IBowlingUserRepository _bowlingUserRepository;

        public TenPinBowlingService()
            : this(new BowlingCardRepository(), new BowlingUserRepository())
        {
        }

        public TenPinBowlingService(IBowlingCardRepository bowlingCardRepository, IBowlingUserRepository bowlingUserRepository)
        {
            _bowlingCardRepository = bowlingCardRepository;
            _bowlingUserRepository = bowlingUserRepository;
        }

        public IList<BowlingScoreCard> CreateUsersScorecard(IList<BowlingUser> bowlingUsers)
        {
            const int framesForGame = 10;
            IList<BowlingScoreCard> bowlingScorecards = new List<BowlingScoreCard>();
            foreach (BowlingUser user in bowlingUsers)
            {
                var bowlingScorecard = new BowlingScoreCard
                {
                    UserId = user.UserId,
                    Frames = new List<BowlingFrame>(framesForGame),
                    Order = 0
                };

                for (int i = 0; i < framesForGame; i++)
                {
                    bowlingScorecard.Frames.Add(new BowlingFrame { FirstBowl = "", SecondBowl = "", FinalBowl = "", Total = 0 });
                }

                bowlingScorecards.Add(bowlingScorecard);
            }

            _bowlingUserRepository.Save(bowlingUsers);
            _bowlingCardRepository.Save(bowlingScorecards);

            return bowlingScorecards;
        }

        public IList<BowlingUser> GetBowlingUser()
        {
            return _bowlingUserRepository.Fetch();
        } 

        public void AddScore(int userId, int frameNumber, int bowlNumber, string score)
        {
            IList<BowlingScoreCard> bowlingScorecards = _bowlingCardRepository.Fetch();

            BowlingScoreCard bowlingScoreCardToAddTo = bowlingScorecards.First(x => x.UserId == userId);

            int frameNumberOffsetAsArray = frameNumber - 1;

            switch (bowlNumber)
            {
                case 1:
                    bowlingScoreCardToAddTo.Frames[frameNumberOffsetAsArray].FirstBowl = score;
                    break;
                case 2:
                    bowlingScoreCardToAddTo.Frames[frameNumberOffsetAsArray].SecondBowl = score;
                    break;
                case 3:
                    bowlingScoreCardToAddTo.Frames[frameNumberOffsetAsArray].FinalBowl = score;
                    break;
            }

            _bowlingCardRepository.Save(bowlingScorecards);
        }

        /// <summary>
        ///  Goes through the bowling card adding up all the total for each from for each user.
        ///  Could do with a refactor but logic all in there and works according to units tests, was quite rushed.
        /// </summary>
        /// <returns></returns>
        public IList<BowlingScoreCard> GetBowlingScoreCardWithTotals()
        {
            IList<BowlingScoreCard> bowlingScorecards = _bowlingCardRepository.Fetch();

            const int framesForGame = 10;

            foreach (BowlingScoreCard bowlingScorecard in bowlingScorecards)
            {
                BowlingFrame previousBowlingFrame = new BowlingFrame { FirstBowl = "", SecondBowl = "", FinalBowl = "", Total = 0 };
                BowlingFrame previousPreviousBowlingFrame = new BowlingFrame { FirstBowl = "", SecondBowl = "", FinalBowl = "", Total = 0 };

                for (int frameNumber = 0; frameNumber < framesForGame; frameNumber++)
                {
                    var bowlingFrame = bowlingScorecard.Frames[frameNumber];
                    if (frameNumber > 0)
                        previousBowlingFrame = bowlingScorecard.Frames[frameNumber - 1];
                    if (frameNumber > 1)
                        previousPreviousBowlingFrame = bowlingScorecard.Frames[frameNumber - 2];

                    int firstBowlScore;
                    bool isFirstBowlNumeric = int.TryParse(bowlingFrame.FirstBowl, out firstBowlScore);

                    int secondBowlScore;
                    bool isSecondBowlNumeric = int.TryParse(bowlingFrame.SecondBowl, out secondBowlScore);

                    int finalBowlScore;
                    bool isFinalBowlNumeric = int.TryParse(bowlingFrame.FinalBowl, out finalBowlScore);

                    if (isFirstBowlNumeric)
                    {
                        if (frameNumber == 0)
                        {
                            bowlingFrame.Total = firstBowlScore;
                        }
                        else
                        {
                            if (bowlingFrame.SecondBowl != "/")
                            {
                                bowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore;
                            }
                            else
                            {
                                bowlingFrame.Total = previousBowlingFrame.Total;
                            }

                            if (previousBowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                previousBowlingFrame.Total = previousBowlingFrame.Total;
                                bowlingFrame.Total = previousBowlingFrame.Total;
                            }
                        }
                    }
                    else if (bowlingFrame.FirstBowl.ToUpper() == "X")
                    {
                        if (previousBowlingFrame.FirstBowl.ToUpper() == "X")
                        {
                            if (previousPreviousBowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                previousPreviousBowlingFrame.Total = previousPreviousBowlingFrame.Total + 10;
                                previousBowlingFrame.Total = previousBowlingFrame.Total + 10;
                            }
                            previousBowlingFrame.Total = previousBowlingFrame.Total + 10;
                        }
                        else if (previousBowlingFrame.SecondBowl.ToUpper() == "/")
                        {
                            previousBowlingFrame.Total = previousBowlingFrame.Total + 10;
                        }
                        bowlingFrame.Total = previousBowlingFrame.Total + 10;
                    }

                    if (isSecondBowlNumeric)
                    {
                        if (previousBowlingFrame.FirstBowl.ToUpper() == "X")
                        {
                            if (previousPreviousBowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                previousPreviousBowlingFrame.Total = previousPreviousBowlingFrame.Total + firstBowlScore;
                                previousBowlingFrame.Total = previousPreviousBowlingFrame.Total + 10;
                            }

                            previousBowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore + secondBowlScore;
                            bowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore + secondBowlScore;
                        }
                        else if (previousBowlingFrame.SecondBowl == "/")
                        {
                            previousBowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore;
                            bowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore + secondBowlScore;
                        }
                        else
                        {
                            bowlingFrame.Total = bowlingFrame.Total + secondBowlScore;
                        }
                    }
                    else if (bowlingFrame.SecondBowl == "/")
                    {
                        if (frameNumber == 0)
                        {
                            bowlingFrame.Total = 10;
                        }
                        else
                        {
                            if (previousBowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                if (previousPreviousBowlingFrame.FirstBowl.ToUpper() == "X")
                                {
                                    previousPreviousBowlingFrame.Total = previousPreviousBowlingFrame.Total + firstBowlScore;
                                    previousBowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore;
                                }
                                previousBowlingFrame.Total = previousBowlingFrame.Total + 10;
                                bowlingFrame.Total = previousBowlingFrame.Total;
                            }
                            else if (previousBowlingFrame.SecondBowl == "/")
                            {
                                previousBowlingFrame.Total = previousBowlingFrame.Total + firstBowlScore;
                                bowlingFrame.Total = previousBowlingFrame.Total;
                            }
                            bowlingFrame.Total = bowlingFrame.Total + 10;
                        }
                    }

                    if (isFinalBowlNumeric)
                    {
                        bowlingFrame.Total = bowlingFrame.Total + finalBowlScore;
                    }
                    else
                    {
                        if (bowlingFrame.FinalBowl.ToUpper() == "X")
                        {
                            if (previousBowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                previousBowlingFrame.Total = previousBowlingFrame.Total + 10;
                            }
                            bowlingFrame.Total = previousBowlingFrame.Total + 10;

                            if (bowlingFrame.SecondBowl.ToUpper() == "X")
                            {
                                bowlingFrame.Total = bowlingFrame.Total + 10;
                            }
                            if (bowlingFrame.FirstBowl.ToUpper() == "X")
                            {
                                bowlingFrame.Total = bowlingFrame.Total + 10;
                            }
                        }
                    }
                }
            }

            return bowlingScorecards;
        }
    }
}