using MyWebApp1.Models;
using MyWebApp1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace MyWebApp1.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        dbGamingEntities _db = new dbGamingEntities();

        // GET: Player
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult BattleView()
        {
            string PlayerId = User.Identity.Name;

            bool battleFinish = false;

            if (TempData["_battleFinish"] != null)
            {
                battleFinish = (bool)TempData["_battleFinish"];
            }

            BattleValueService battleValueService = new BattleValueService(battleFinish);

            BattleValue battleValue = battleValueService.CalculateBattleValues(PlayerId);

            return View(battleValue);
        }

        public ActionResult BattleFinish()
        {
            TempData["_battleFinish"] = true;

            return RedirectToAction("BattleView", "Player");
        }

        public ActionResult MaterialEssence()
        {
            TempData["_Material"] = 0;
            return RedirectToAction("MaterialView", "Player");
        }

        public ActionResult MaterialCrystal()
        {
            TempData["_Material"] = 1;
            return RedirectToAction("MaterialView", "Player");
        }

        public ActionResult MaterialView()
        {
            int MaterialId = TempData["_Material"] != null ? (int)TempData["_Material"] : 0;
            string PlayerId = User.Identity.Name;

            BattleValueService battleValueService = new BattleValueService();
            CalculateService calculateService = new CalculateService();

            string[] materialName = calculateService.GetMaterialName(MaterialId);
            ViewBag.MaterialName = materialName[0];
            ViewBag.MaterialDesc = materialName[1];
            ViewBag.MaterialAmount = calculateService.GetPlayerMaterial(PlayerId)[MaterialId];

            var affixList = calculateService.CalculateAffixValue(PlayerId, MaterialId);

            return View(affixList);
        }

        public ActionResult MaterialLvAdd(int AffixId, int AffixLv, int terms, int MaterialId)
        {
            string PlayerId = User.Identity.Name;

            CalculateService calculateService = new CalculateService();

            string showMessage = calculateService.PlayerAffixUpdate(PlayerId, AffixId, AffixLv, terms);

            TempData["ShowMessage"] = showMessage;

            TempData["_Material"] = MaterialId;

            TempData["_AffixId"] = AffixId;

            return RedirectToAction("MaterialView", "Player");
        }

        public ActionResult LevelUpView()
        {
            string PlayerId = User.Identity.Name;

            CalculateService calculateService = new CalculateService();

            LevelUpModel levelUpModel = calculateService.CalculateLevelUpValue(PlayerId);

            return View(levelUpModel);
        }

        public ActionResult LevelUpdate()
        {
            string PlayerId = User.Identity.Name;

            CalculateService calculateService = new CalculateService();

            string showMessage = calculateService.PlayerLevelUpdate(PlayerId);

            TempData["ShowMessage"] = showMessage;

            return RedirectToAction("LevelUpView", "Player");
        }

        public ActionResult PlayerInfoView()
        {
            string PlayerId = User.Identity.Name;

            CalculateService calculateService = new CalculateService();

            PlayerInfo playerInfo = calculateService.DisplayPlayerInfo(PlayerId);

            return View(playerInfo);
        }

        public ActionResult ChangeName()
        {
            string PlayerId = User.Identity.Name;

            var player = _db.Player.Where(m => m.PlayerId == PlayerId).FirstOrDefault();
            if (player != null)
            {
                ViewBag.Name = player.PlayerName;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(string name)
        {
            string PlayerId = User.Identity.Name;

            var player = _db.Player.Where(m => m.PlayerId == PlayerId).FirstOrDefault();

            if (player != null)
            {
                player.PlayerName = name;

                _db.SaveChanges();

                return RedirectToAction("PlayerInfoView");
            }

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            //如果資料驗證未通過則回傳原本的View
            if (!ModelState.IsValid)
            {
                return View();
            }

            string PlayerId = User.Identity.Name;

            var player = _db.Player.Where(m => m.PlayerId == PlayerId && m.Password == changePasswordModel.OldPassword).FirstOrDefault();
            if (player == null)
            {
                ViewBag.Message = "舊密碼輸入錯誤";
                return View();
            }
            else
            {
                if (changePasswordModel.OldPassword == changePasswordModel.NewPassword)
                {
                    ViewBag.Message = "舊密碼與新密碼不能相同";
                    return View();
                }

                player.Password = changePasswordModel.NewPassword;

                _db.SaveChanges();

                FormsAuthentication.SignOut();

                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult PlayerRank()
        {
            var players = _db.Player.OrderByDescending(m => m.BestStage).ThenBy(m => m.Level).Take(50).ToList();

            return View("../Home/PlayerRank", "_LayoutPlayer", players);
        }

    }
    
}