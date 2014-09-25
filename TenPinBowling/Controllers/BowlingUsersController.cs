using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenPinBowling.Models;
using TenPinBowling.Services;

namespace TenPinBowling.Controllers
{
    public class BowlingUsersController : Controller
    {
        private ITenPinBowlingService _tenPinBowlingService;

        public BowlingUsersController() : this(new TenPinBowlingService())
        {
        }

        public BowlingUsersController(ITenPinBowlingService tenPinBowlingService)
        {
            _tenPinBowlingService = tenPinBowlingService;
        }

        [HttpGet, ActionName("BowlingUsers")]
        public ActionResult GetUsers()
        {
            var bowlingUsers = _tenPinBowlingService.GetBowlingUser();

            return Json(bowlingUsers, JsonRequestBehavior.AllowGet);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // TODO http://stackoverflow.com/questions/2906754/how-can-i-supply-an-antiforgerytoken-when-posting-json-data-using-ajax
        [HttpPost, ActionName("BowlingUsers")]
        public ActionResult AddUsers(IList<BowlingUser> bowlingUsers)
        {
            //TODO validation to be added
            _tenPinBowlingService.CreateUsersScorecard(bowlingUsers);

            return Json(bowlingUsers);
        }
	}
}