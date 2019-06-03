using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServerApp.Controllers
{
    public class HomeController : Controller
    {
        PWContext db = new PWContext();
        public ActionResult Index()
        {
            //var users = db.Users;
            //ViewBag.Users = users;
            
            //if(authoriz)
            return View();
            //else Login();
        } 
        public ActionResult NewTransaction()
        {
            return View();
        }
        [HttpPost]
        public string NewTransaction(Transaction tr)
        {
            return "Транзакция, трали-вали";
        }
        public ActionResult RepeatTransaction()
        {
            var tr = db.Transactions.FirstOrDefault();//where(id==)
            ViewBag.LastTransaction = tr;
            return View();
        }
        public ActionResult ListTransaction()
        {
            return View();
        }


    }
}