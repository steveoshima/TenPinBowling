using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TenPinBowling.Models;
using TenPinBowling.Repositories;
using TenPinBowling.Services;

namespace TenPinBowling.Tests.Services
{
    [TestClass]
    public class BowlingServiceTest2
    {
        [TestMethod]
        public void CreateNewScoreCardTest()
        {
            IList<BowlingScoreCard> newScorecard = SetupBowlingCard();
            // Assert
            Assert.IsNotNull(newScorecard);
        }

        private IList<BowlingUser> SetUpBowlingUsers()
        {
            var bowlingUsers = new List<BowlingUser>();
            bowlingUsers.Add(new BowlingUser { UserId = 1, UserName = "John" });
            bowlingUsers.Add(new BowlingUser { UserId = 2, UserName = "Mary" });
            bowlingUsers.Add(new BowlingUser { UserId = 3, UserName = "Kim" });
            bowlingUsers.Add(new BowlingUser { UserId = 4, UserName = "Leo" });
            return bowlingUsers;
        } 

        private IList<BowlingScoreCard> SetupBowlingCard()
        {
            // Create mock repository
            IBowlingCardRepository mockRepository = Mock.Of<IBowlingCardRepository>();
            IBowlingUserRepository mockRepository2 = Mock.Of<IBowlingUserRepository>();
            // Arrange
            TenPinBowlingService service = new TenPinBowlingService(mockRepository, mockRepository2);
            // Test data
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();
            // Act
            IList<BowlingScoreCard> newScorecard = service.CreateUsersScorecard(bowlingUsers);
            return newScorecard;
        }

        [TestMethod]
        public void NoSparesOrStrikes_Test()
        {
            //Setup
            IList<BowlingScoreCard> newScorecard = SetupBowlingCard();
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();

            // Create mock repository
            var mock = new Mock<IBowlingCardRepository>();
            mock.Setup(bowlingCardRepository => bowlingCardRepository.Fetch()).Returns(newScorecard);
            IBowlingCardRepository mockRepository2 = mock.Object;
            var mock2 = new Mock<IBowlingUserRepository>();
            mock2.Setup(bowlingUserRepository => bowlingUserRepository.Fetch()).Returns(bowlingUsers);
            IBowlingUserRepository mockRepository3 = mock2.Object;
            // Arrange
            TenPinBowlingService service2 = new TenPinBowlingService(mockRepository2, mockRepository3);

            service2.AddScore(1, 1, 1, "9");
            service2.AddScore(1, 1, 2, "0");
            service2.AddScore(1, 2, 1, "3");
            service2.AddScore(1, 2, 2, "5");
            service2.AddScore(1, 3, 1, "6");
            service2.AddScore(1, 3, 2, "1");
            service2.AddScore(1, 4, 1, "3");
            service2.AddScore(1, 4, 2, "6");
            service2.AddScore(1, 5, 1, "8");
            service2.AddScore(1, 5, 2, "1");
            service2.AddScore(1, 6, 1, "5");
            service2.AddScore(1, 6, 2, "3");
            service2.AddScore(1, 7, 1, "2");
            service2.AddScore(1, 7, 2, "5");
            service2.AddScore(1, 8, 1, "8");
            service2.AddScore(1, 8, 2, "0");
            service2.AddScore(1, 9, 1, "7");
            service2.AddScore(1, 9, 2, "1");
            service2.AddScore(1, 10, 1, "8");
            service2.AddScore(1, 10, 2, "1");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;
            int totalF5 = scoreCard.Frames[4].Total;
            int totalF6 = scoreCard.Frames[5].Total;
            int totalF7 = scoreCard.Frames[6].Total;
            int totalF8 = scoreCard.Frames[7].Total;
            int totalF9 = scoreCard.Frames[8].Total;
            int totalF10 = scoreCard.Frames[9].Total;

            Assert.AreEqual(9, totalF1);
            Assert.AreEqual(17, totalF2);
            Assert.AreEqual(24, totalF3);
            Assert.AreEqual(33, totalF4);
            Assert.AreEqual(42, totalF5);
            Assert.AreEqual(50, totalF6);
            Assert.AreEqual(57, totalF7);
            Assert.AreEqual(65, totalF8);
            Assert.AreEqual(73, totalF9);
            Assert.AreEqual(82, totalF10);
        }

        [TestMethod]
        public void SparesIncluded_Test()
        {
            //Setup
            IList<BowlingScoreCard> newScorecard = SetupBowlingCard();
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();

            // Create mock repository
            var mock = new Mock<IBowlingCardRepository>();
            mock.Setup(bowlingCardRepository => bowlingCardRepository.Fetch()).Returns(newScorecard);
            IBowlingCardRepository mockRepository2 = mock.Object;
            var mock2 = new Mock<IBowlingUserRepository>();
            mock2.Setup(bowlingUserRepository => bowlingUserRepository.Fetch()).Returns(bowlingUsers);
            IBowlingUserRepository mockRepository3 = mock2.Object;
            // Arrange
            TenPinBowlingService service2 = new TenPinBowlingService(mockRepository2, mockRepository3);

            service2.AddScore(2, 1, 1, "9");
            service2.AddScore(2, 1, 2, "0");
            service2.AddScore(2, 2, 1, "3");
            service2.AddScore(2, 2, 2, "/");
            service2.AddScore(2, 3, 1, "6");
            service2.AddScore(2, 3, 2, "1");
            service2.AddScore(2, 4, 1, "3");
            service2.AddScore(2, 4, 2, "/");
            service2.AddScore(2, 5, 1, "8");
            service2.AddScore(2, 5, 2, "1");
            service2.AddScore(2, 6, 1, "5");
            service2.AddScore(2, 6, 2, "/");
            service2.AddScore(2, 7, 1, "0");
            service2.AddScore(2, 7, 2, "/");
            service2.AddScore(2, 8, 1, "8");
            service2.AddScore(2, 8, 2, "0");
            service2.AddScore(2, 9, 1, "7");
            service2.AddScore(2, 9, 2, "/");
            service2.AddScore(2, 10, 1, "8");
            service2.AddScore(2, 10, 2, "/");
            service2.AddScore(2, 10, 3, "8");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 2);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;
            int totalF5 = scoreCard.Frames[4].Total;
            int totalF6 = scoreCard.Frames[5].Total;
            int totalF7 = scoreCard.Frames[6].Total;
            int totalF8 = scoreCard.Frames[7].Total;
            int totalF9 = scoreCard.Frames[8].Total;
            int totalF10 = scoreCard.Frames[9].Total;

            Assert.AreEqual(9, totalF1);
            Assert.AreEqual(25, totalF2);
            Assert.AreEqual(32, totalF3);
            Assert.AreEqual(50, totalF4);
            Assert.AreEqual(59, totalF5);
            Assert.AreEqual(69, totalF6);
            Assert.AreEqual(87, totalF7);
            Assert.AreEqual(95, totalF8);
            Assert.AreEqual(113, totalF9);
            Assert.AreEqual(131, totalF10);
        }

        [TestMethod]
        public void SparesAndStrikes_Test()
        {
            //Setup
            IList<BowlingScoreCard> newScorecard = SetupBowlingCard();
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();

            // Create mock repository
            var mock = new Mock<IBowlingCardRepository>();
            mock.Setup(bowlingCardRepository => bowlingCardRepository.Fetch()).Returns(newScorecard);
            IBowlingCardRepository mockRepository2 = mock.Object;
            var mock2 = new Mock<IBowlingUserRepository>();
            mock2.Setup(bowlingUserRepository => bowlingUserRepository.Fetch()).Returns(bowlingUsers);
            IBowlingUserRepository mockRepository3 = mock2.Object;
            // Arrange
            TenPinBowlingService service2 = new TenPinBowlingService(mockRepository2, mockRepository3);

            service2.AddScore(3, 1, 1, "X");
            service2.AddScore(3, 2, 1, "3");
            service2.AddScore(3, 2, 2, "/");
            service2.AddScore(3, 3, 1, "6");
            service2.AddScore(3, 3, 2, "1");
            service2.AddScore(3, 4, 1, "X");
            service2.AddScore(3, 5, 1, "X");
            service2.AddScore(3, 6, 1, "X");
            service2.AddScore(3, 7, 1, "2");
            service2.AddScore(3, 7, 2, "/");
            service2.AddScore(3, 8, 1, "9");
            service2.AddScore(3, 8, 2, "0");
            service2.AddScore(3, 9, 1, "7");
            service2.AddScore(3, 9, 2, "/");
            service2.AddScore(3, 10, 1, "X");
            service2.AddScore(3, 10, 2, "X");
            service2.AddScore(3, 10, 3, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 3);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;
            int totalF5 = scoreCard.Frames[4].Total;
            int totalF6 = scoreCard.Frames[5].Total;
            int totalF7 = scoreCard.Frames[6].Total;
            int totalF8 = scoreCard.Frames[7].Total;
            int totalF9 = scoreCard.Frames[8].Total;
            int totalF10 = scoreCard.Frames[9].Total;

            Assert.AreEqual(20, totalF1);
            Assert.AreEqual(36, totalF2);
            Assert.AreEqual(43, totalF3);
            Assert.AreEqual(73, totalF4);
            Assert.AreEqual(95, totalF5);
            Assert.AreEqual(115, totalF6);
            Assert.AreEqual(134, totalF7);
            Assert.AreEqual(143, totalF8);
            Assert.AreEqual(163, totalF9);
            Assert.AreEqual(193, totalF10);
        }

        [TestMethod]
        public void PerfectGame_Test()
        {
            //Setup
            IList<BowlingScoreCard> newScorecard = SetupBowlingCard();
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();

            // Create mock repository
            var mock = new Mock<IBowlingCardRepository>();
            mock.Setup(bowlingCardRepository => bowlingCardRepository.Fetch()).Returns(newScorecard);
            IBowlingCardRepository mockRepository2 = mock.Object;
            var mock2 = new Mock<IBowlingUserRepository>();
            mock2.Setup(bowlingUserRepository => bowlingUserRepository.Fetch()).Returns(bowlingUsers);
            IBowlingUserRepository mockRepository3 = mock2.Object;
            // Arrange
            TenPinBowlingService service2 = new TenPinBowlingService(mockRepository2, mockRepository3);

            service2.AddScore(4, 1, 1, "X");
            service2.AddScore(4, 2, 1, "X");
            service2.AddScore(4, 3, 1, "X");
            service2.AddScore(4, 4, 1, "X");
            service2.AddScore(4, 5, 1, "X");
            service2.AddScore(4, 6, 1, "X");
            service2.AddScore(4, 7, 1, "X");
            service2.AddScore(4, 8, 1, "X");
            service2.AddScore(4, 9, 1, "X");
            service2.AddScore(4, 10, 1, "X");
            service2.AddScore(4, 10, 2, "X");
            service2.AddScore(4, 10, 3, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 4);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;
            int totalF5 = scoreCard.Frames[4].Total;
            int totalF6 = scoreCard.Frames[5].Total;
            int totalF7 = scoreCard.Frames[6].Total;
            int totalF8 = scoreCard.Frames[7].Total;
            int totalF9 = scoreCard.Frames[8].Total;
            int totalF10 = scoreCard.Frames[9].Total;

            Assert.AreEqual(30, totalF1);
            Assert.AreEqual(60, totalF2);
            Assert.AreEqual(90, totalF3);
            Assert.AreEqual(120, totalF4);
            Assert.AreEqual(150, totalF5);
            Assert.AreEqual(180, totalF6);
            Assert.AreEqual(210, totalF7);
            Assert.AreEqual(240, totalF8);
            Assert.AreEqual(270, totalF9);
            Assert.AreEqual(300, totalF10);
        }
    }

}
