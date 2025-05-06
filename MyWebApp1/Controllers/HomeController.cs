using MyWebApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyWebGame.Controllers
{
    public class HomeController : Controller
    {
        dbGamingEntities _db = new dbGamingEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string PlayerId, string Password)
        {
            var member = _db.Player.Where(x => x.PlayerId == PlayerId && x.Password == Password).FirstOrDefault();
            if (member == null)
            {
                ViewBag.Message = "帳號或密碼錯誤，請重新確認輸入";
                return View();
            }

            FormsAuthentication.RedirectFromLoginPage(PlayerId, true);

            return RedirectToAction("Index", "Player");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Register register)
        {
            //如果資料驗證未通過則回傳原本的View
            if (!ModelState.IsValid)
            {
                return View();
            }

            var player = _db.Player.Where(m => m.PlayerId == register.PlayerId).FirstOrDefault();
            if (player == null)
            {
                var table_Player = new Player();
                table_Player.PlayerId = register.PlayerId;
                table_Player.Password = register.Password;
                table_Player.PlayerName = register.PlayerName;
                table_Player.CreateDate = DateTime.Now;
                table_Player.BestStage = 0;
                table_Player.CurrentStage = 0;
                table_Player.Level = 0;
                _db.Player.Add(table_Player);
                _db.SaveChanges();

                TempData["RegisterMessage"] = "註冊成功!";

                return RedirectToAction("Login");
            }

            ViewBag.Message = "帳號已被使用，請重新註冊";
            return View();
        }

        public ActionResult PlayerRank()
        {
            var players = _db.Player.OrderByDescending(m => m.BestStage).ThenBy(m => m.Level).Take(50).ToList();

            return View(players);
        }

    }
}