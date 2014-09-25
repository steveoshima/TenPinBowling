using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TenPinBowling.Controllers;
using TenPinBowling.Models;
using TenPinBowling.Repositories;
using TenPinBowling.Services;

namespace TenPinBowling.Tests.Services
{
    [TestClass]
    public class BowlingServiceTest
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
            bowlingUsers.Add(new BowlingUser { UserId = 1, UserName = "Bert" });
            bowlingUsers.Add(new BowlingUser { UserId = 2, UserName = "Sam" });
            return bowlingUsers;
        } 

        private IList<BowlingScoreCard> SetupBowlingCard()
        {
            // Test user data
            IList<BowlingUser> bowlingUsers = SetUpBowlingUsers();

            // Create mock repository
            IBowlingCardRepository mockRepository = Mock.Of<IBowlingCardRepository>();
            IBowlingUserRepository mockRepository2 = Mock.Of<IBowlingUserRepository>();
            // Arrange
            TenPinBowlingService service = new TenPinBowlingService(mockRepository, mockRepository2);
            
            // Act
            IList<BowlingScoreCard> newScorecard = service.CreateUsersScorecard(bowlingUsers);
            return newScorecard;
        }
            
        [TestMethod]
        public void FirstBowl_FirstFrameTest()
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
            service2.AddScore(1,1,1,"8");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].FirstBowl;
            int total = scoreCard.Frames[0].Total;

            Assert.AreEqual("8", score);
            Assert.AreEqual(8, total);

        }

        [TestMethod]
        public void StrikeFirstFrame_Test()
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
            service2.AddScore(1, 1, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].FirstBowl;
            int total = scoreCard.Frames[0].Total;

            Assert.AreEqual("X", score);
            Assert.AreEqual(10, total);
        }

        [TestMethod]
        public void StrikeFirstFrame_SecondFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "3");
            service2.AddScore(1, 2, 2, "2");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].FirstBowl;
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;

            Assert.AreEqual("X", score);
            Assert.AreEqual(15, totalF1);
            Assert.AreEqual(20, totalF2);
        }

        [TestMethod]
        public void StrikeFirstFrame_StrikeSecondFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;

            Assert.AreEqual(20, totalF1);
            Assert.AreEqual(30, totalF2);
        }

        [TestMethod]
        public void StrikeFirstFrame_StrikeSecondFrame_ThirdFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "X");
            //third frame
            service2.AddScore(1, 3, 1, "6");
            service2.AddScore(1, 3, 2, "2");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;

            Assert.AreEqual(26, totalF1);
            Assert.AreEqual(44, totalF2);
            Assert.AreEqual(52, totalF3);
        }

        [TestMethod]
        public void StrikeFirstFrame_StrikeSecondFrame_StrikeThirdFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "X");
            //third frame
            service2.AddScore(1, 3, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;

            Assert.AreEqual(30, totalF1);
            Assert.AreEqual(50, totalF2);
            Assert.AreEqual(60, totalF3);
        }

        [TestMethod]
        public void StrikeFirstFrame_StrikeSecondFrame_StrikeThirdFrame_ForthFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "X");
            //third frame
            service2.AddScore(1, 3, 1, "X");
            //forth frame
            service2.AddScore(1, 4, 1, "6");
            service2.AddScore(1, 4, 2, "2");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;

            Assert.AreEqual(30, totalF1);
            Assert.AreEqual(56, totalF2);
            Assert.AreEqual(74, totalF3);
            Assert.AreEqual(82, totalF4);
        }

        [TestMethod]
        public void StrikeFirstFrame_StrikeSecondFrame_StrikeThirdFrame_StrikeForthFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "X");
            //second frame
            service2.AddScore(1, 2, 1, "X");
            //third frame
            service2.AddScore(1, 3, 1, "X");
            //forth frame
            service2.AddScore(1, 4, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;

            Assert.AreEqual(30, totalF1);
            Assert.AreEqual(60, totalF2);
            Assert.AreEqual(80, totalF3);
            Assert.AreEqual(90, totalF4);
        }

        [TestMethod]
        public void SecondBowl_FirstFrameTest()
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
            service2.AddScore(1, 1, 1, "4");
            service2.AddScore(1, 1, 2, "4");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].SecondBowl;
            int total = scoreCard.Frames[0].Total;

            Assert.AreEqual("4", score);
            Assert.AreEqual(8, total);
        }

        [TestMethod]
        public void SpareFirstFrame_SecondFrame_Test()
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
            //First frame
            service2.AddScore(1, 1, 1, "3");
            service2.AddScore(1, 1, 2, "/");
            //Second frame
            service2.AddScore(1, 2, 1, "4");
            service2.AddScore(1, 2, 2, "5");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].SecondBowl;
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;

            Assert.AreEqual("/", score);
            Assert.AreEqual(14, totalF1);
            Assert.AreEqual(23, totalF2);
        }

        [TestMethod]
        public void SpareFirstFrame_SpareSecondFrame_ThirdFrameTest()
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
            //First frame
            service2.AddScore(1, 1, 1, "3");
            service2.AddScore(1, 1, 2, "/");
            //Second frame
            service2.AddScore(1, 2, 1, "2");
            service2.AddScore(1, 2, 2, "/");
            //Third frame
            service2.AddScore(1, 3, 1, "4");
            service2.AddScore(1, 3, 2, "3");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[0].SecondBowl;
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;

            Assert.AreEqual("/", score);
            Assert.AreEqual(12, totalF1);
            Assert.AreEqual(26, totalF2);
            Assert.AreEqual(33, totalF3);
        }

        [TestMethod]
        public void FirstFrame_SpareSecondFrame_SpareThirdFrame_Forth_Test()
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
            //First frame
            service2.AddScore(1, 1, 1, "9");
            service2.AddScore(1, 1, 2, "0");
            //Second frame
            service2.AddScore(1, 2, 1, "2");
            service2.AddScore(1, 2, 2, "/");
            //Third frame
            service2.AddScore(1, 3, 1, "4");
            service2.AddScore(1, 3, 2, "/");
            //Third frame
            service2.AddScore(1, 4, 1, "6");
            service2.AddScore(1, 4, 2, "1");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;
            int totalF4 = scoreCard.Frames[3].Total;

            Assert.AreEqual(9, totalF1);
            Assert.AreEqual(23, totalF2);
            Assert.AreEqual(39, totalF3);
            Assert.AreEqual(46, totalF4);
        }

        [TestMethod]
        public void FirstFrame_SecondFrame_Test()
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
            
            //first frame
            service2.AddScore(1, 1, 1, "3");
            service2.AddScore(1, 1, 2, "5");

            //second frame
            service2.AddScore(1, 2, 1, "7");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[1].FirstBowl;
            int total = scoreCard.Frames[1].Total;

            Assert.AreEqual("7", score);
            Assert.AreEqual(15, total);
        }

        [TestMethod]
        public void FirstBowl_SecondBowl_StrikeSecondFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "3");
            service2.AddScore(1, 1, 2, "5");

            //second frame
            service2.AddScore(1, 2, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);
            string score = scoreCard.Frames[1].FirstBowl;
            int total = scoreCard.Frames[1].Total;

            Assert.AreEqual("X", score);
            Assert.AreEqual(18, total);
        }

        [TestMethod]
        public void FirstBowl_SecondBowl_StrikeSecondFrame_StrikeThirdFrame_Test()
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

            //first frame
            service2.AddScore(1, 1, 1, "3");
            service2.AddScore(1, 1, 2, "5");

            //second frame
            service2.AddScore(1, 2, 1, "X");

            //third frame
            service2.AddScore(1, 3, 1, "X");

            IList<BowlingScoreCard> updatedScorecard = service2.GetBowlingScoreCardWithTotals();

            // Assert
            var scoreCard = updatedScorecard.First(x => x.UserId == 1);

            int totalF1 = scoreCard.Frames[0].Total;
            int totalF2 = scoreCard.Frames[1].Total;
            int totalF3 = scoreCard.Frames[2].Total;

            Assert.AreEqual(8, totalF1);
            Assert.AreEqual(28, totalF2);
            Assert.AreEqual(38, totalF3);
        }

    }
}
