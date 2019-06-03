using ParrotWingsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParrotWingsWebApp.Controllers
{
    /// <summary>
    /// Создание новых пользователей, либо вход существующих
    /// </summary>
    public class AccountController : Controller
    {
        public static string pass;
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginAccount logUser)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (PWContext db = new PWContext())
                {
                    user= db.Users.FirstOrDefault(u => u.EMail == logUser.EMail && u.Password==logUser.Pass);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.EMail, true);
                    pass = user.Password;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неверно указаны почта или пароль пользователя!");
                }
            }
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterAccount newUser)
        {
            if (ModelState.IsValid)
            {
                User user=null;
                using (PWContext db=new PWContext())
                {
                    user = db.Users.FirstOrDefault(u => u.EMail == newUser.EMail);                    
                }
                if (user == null)
                {
                    using (PWContext db = new PWContext())
                    {
                        db.Users.Add(new User { Name = newUser.Name, EMail = newUser.EMail, Password = newUser.Pass, Balance = 500.0 });
                        db.SaveChanges();
                        user = db.Users.Where(u => newUser.EMail == u.EMail && newUser.Pass == u.Password).FirstOrDefault();                            
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.EMail,true);
                        pass = user.Password;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь уже зарегистрирован!");
                }
            }
            return View(newUser); 
        }        
        public ActionResult ChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(ChangePass changePass)
        {
            if (ModelState.IsValid)
            {
                if (changePass.OldPassword == pass)
                {
                    using (PWContext db = new PWContext())
                    {
                        db.Users.FirstOrDefault(u => u.EMail == User.Identity.Name).Password = changePass.NewPassword;
                        db.SaveChanges();
                    }
                    return View("ChangePassSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Неверные данные!");
                }
            }
            return View(changePass);
        }
        public ActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPass( string Mail)
        {
            User user;
            using (PWContext db = new PWContext())
            {
                user = db.Users.FirstOrDefault(u => u.EMail == Mail);
                if (user != null)
                {
                    db.Users.FirstOrDefault(u => u.EMail == Mail).Password = GeneratePass();
                    db.SaveChanges();
                    //отправка письма
                    return View("ForgotPassSuccess");
                }
                else
                {
                    ModelState.AddModelError("","Такой пользователь не обнаружен!");
                }
                return View();
            }
        }
        string GeneratePass()
        {
            //генерируемый рандомный набор символов
            //по-умолчанию просто "00000"
            return "00000";
        }
    }
}