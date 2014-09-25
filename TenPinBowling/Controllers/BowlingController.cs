using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenPinBowling.Models;
using TenPinBowling.Repositories;
using TenPinBowling.Services;

namespace TenPinBowling.Controllers
{
    public class BowlingController : Controller
    {
        private ITenPinBowlingService _tenPinBowlingService;

        public BowlingController() : this(new TenPinBowlingService())
        {
        }

        public BowlingController(ITenPinBowlingService tenPinBowlingService)
        {
            _tenPinBowlingService = tenPinBowlingService;
        }

        //
        // GET: /Bowling/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("BowlingCards")]
        public ActionResult Get()
        {
            var bowlingScoreCards = _tenPinBowlingService.GetBowlingScoreCardWithTotals();

            return Json(bowlingScoreCards, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("BowlingScore")]
        public ActionResult Post([Bind(Include = "UserId, FrameNumber, BowlNumber, Score")] BowlingTurn bowlingTurn)
        {
            _tenPinBowlingService.AddScore(bowlingTurn.UserId, bowlingTurn.FrameNumber, bowlingTurn.BowlNumber, bowlingTurn.Score);

            return Json(bowlingTurn);
        } 

	}
}