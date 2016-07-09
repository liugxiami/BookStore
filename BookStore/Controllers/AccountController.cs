using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            BookStoreEntities dbContext = new BookStoreEntities();
            Customer user = (from c in dbContext.Customers
                             where c.UserName.Equals(Username) &&
                             c.PassWord.Equals(Password)
                             select c).FirstOrDefault();
            if (user != null)
            {
                Session["UserId"] = user.Id.ToString();
                Session["UserName"] = user.UserName.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or Password is wrong.");
                return View();
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer user)
        {
            if (user != null && ModelState.IsValid)
            {
                using (BookStoreEntities dbContext = new BookStoreEntities())
                {
                    var allusers = dbContext.Customers;
                    if (allusers.Count() == 0)
                    {
                        user.Id = 1;
                    }
                    else
                    {
                        user.Id = allusers.Max(u => u.Id) + 1;
                    }
                    allusers.Add(user);
                    dbContext.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.FirstName + " " + user.LastName + " successfully registered.";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}