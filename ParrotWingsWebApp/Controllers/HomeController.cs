using ParrotWingsWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ParrotWingsWebApp.Controllers
{
    public class HomeController : Controller
    {
        static User[] otherUsers = null;
        static User user = null;
        static List<Transaction> transactions = new List<Transaction>();
        static int sortItem=1;//сортировка по умолчанию-по дате

        /// <summary>
        /// Метод для организации главной рабочей страницы пользователя
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (PWContext db = new PWContext())
                {
                    user = db.Users.FirstOrDefault(u => u.EMail == User.Identity.Name);
                    AccountController.pass = user.Password;
                    otherUsers = db.Users.Where(u => u.EMail != User.Identity.Name).ToArray();
                    transactions = db.Transactions.Where(t => t.SenderId == user.Id || t.ReceiverId == user.Id).ToList();
                    SortTransactions();
                    ViewBag.CurrentUser = user;
                    ViewBag.Balance = user.Balance;
                    ViewBag.Transactions = transactions;
                    ViewBag.OtherUsers = otherUsers;
                }
                return View("Index");
            }
            return View("IndexNotLogged");
        }
        /// <summary>
        /// Повтор выбранного платежа с заполнением полей данными из проведенной транзакции
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Repeat(int? id)
        {
            Transaction tr=null;
            string senderMail;
            using (PWContext db = new PWContext())
            {
                tr = db.Transactions.FirstOrDefault(t => t.Id == id);
                senderMail = db.Users.FirstOrDefault(u => u.Id == tr.ReceiverId).EMail;
            }
            ViewBag.RepeatUser = senderMail;
            ViewBag.Balance = user.Balance;
            return View("Transaction", tr);
        }
        /// <summary>
        /// Сортировка передаваемого в представление списка транзакций данного пользователя по различным полям
        /// </summary>
        private void SortTransactions()
        {
            List<Transaction> tr=new List<Transaction>();
            if (sortItem == 1)
            {
                tr = transactions.OrderByDescending(s => s.Date).ToList();
            }
            else if (sortItem == -1)
            {
                tr = transactions.OrderBy(s => s.Date).ToList();
            }
            else if (sortItem == 2)
            {
                tr = transactions.OrderByDescending(s => s.Sum).ToList();
            }
            else if (sortItem == -2)
            {
                tr = transactions.OrderBy(s => s.Sum).ToList();
            }
            else if (sortItem == 3)
            {

                tr = CorrespondentSort(false);
            }
            else if (sortItem == -3)
            {
                tr = CorrespondentSort(true);
            }
            transactions = tr;
        }
        /// <summary>
        /// Автогенерируемы метод. Отображает представление, доступное только залогиненым пользователям
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult About()
        {
            return View();
        }
        /// <summary>
        /// Автогенерируемы метод. Отображает представление, доступное всем  пользователям
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }
        /// <summary>
        /// Начало новой транзакции
        /// </summary>
        /// <returns></returns>
        public ActionResult Transaction()
        {
            ViewBag.Balance = user.Balance;            
            return View();
        }        
        /// <summary>
        /// Post-метод для выполнения транзакции при удачной валидациии, иначе отправка причин отказа
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="Mail"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Transaction(Transaction tr,string Mail)
        {
            if (ModelState.IsValid)
            {
                if(user.Balance < tr.Sum || tr.Sum<0)
                {
                    ModelState.AddModelError("", "Неверная сумма!");                    
                }
                else if (otherUsers.FirstOrDefault(u => u.EMail == Mail)==null)
                {
                    ModelState.AddModelError("", "Такого получателя в сети не зарегистрировано!");
                }
                else
                {
                    try
                    {
                        using (PWContext db = new PWContext())
                        {
                            Transaction newTr = new Transaction { Date = DateTime.Now                                                                      
                                                                      , Sum = tr.Sum
                                                                      , SenderId = user.Id
                                                                      , ReceiverId= db.Users.FirstOrDefault(u => u.EMail == Mail).Id
                                                                      , ReceiverBalance= db.Users.FirstOrDefault(u => u.EMail == Mail).Balance+tr.Sum
                                                                      , SenderBalance = user.Balance - tr.Sum };
                            

                            db.Users.FirstOrDefault(u => u.EMail == User.Identity.Name).Balance -= tr.Sum;
                            db.Users.FirstOrDefault(u => u.EMail == Mail).Balance += tr.Sum;
                            db.Transactions.Add(newTr);

                            db.SaveChanges();                            
                        }

                        return View("TransactionSuccess");
                    }
                    catch
                    {
                        return View("TransactionFailed");
                    }
                }
            }            
            else
            {
                ModelState.AddModelError("", "Заполенены не все поля!");
            }
            ViewBag.Balance = user.Balance;
            return View(tr);

        }
        /// <summary>
        /// Представление списка слов для автодополнения имени адресата транзакции
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult AutocompleteSearch(string term)
        {
            var models = otherUsers.Where(u => u.EMail.Contains(term)).Select(u => new { value = u.EMail });  

            return Json(models, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Выход из учетной записи для браузера и зануление статических полей
        /// </summary>
        /// <returns></returns>
        public ActionResult Exit()
        {
            FormsAuthentication.SignOut();
            user = null;
            otherUsers = null;
            transactions = null;
            sortItem = 1;
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Определяет, по какому полю необходимо сортировать список транзакций
        /// </summary>
        /// <param name="sortId"></param>
        /// <returns></returns>
        public ActionResult SortMethod(int sortId)
        {
            if (sortId == sortItem)
            {
                sortItem = -sortId;
            }
            else
            {
                sortItem = sortId;
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Вспомогательный метод для сортировки электронных почт из списка транзакций по двум параллельным столбикам
        /// </summary>
        /// <param name="desc"></param>
        /// <returns></returns>
        List<Transaction> CorrespondentSort(bool desc)
        {
            List<(string, int)> tempSet = new List<(string, int)>();
            foreach (var item in transactions)
            {
                if (item.SenderId == user.Id)
                {
                    tempSet.Add((otherUsers.FirstOrDefault(u=>u.Id==item.ReceiverId).EMail, item.Id));
                }
                else
                {
                    tempSet.Add((otherUsers.FirstOrDefault(u => u.Id == item.SenderId).EMail, item.Id));
                }
            }
            IOrderedEnumerable<(string,int)> elements = null;
            if (!desc)
            {
                elements= tempSet.OrderBy(i=>i.Item1);
            }
            else
            {
                elements = tempSet.OrderByDescending(i => i.Item1);
            }
            List<Transaction> tempTr = new List<Transaction>();
            foreach (var item in elements)
            {
                tempTr.Add(transactions.FirstOrDefault(t => t.Id == item.Item2));
            }
            return tempTr;
        }
    }   
}