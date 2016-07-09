using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.Net;
using System.Net.Mail;

namespace BookStore.Controllers
{
    public class CheckOutController : Controller
    {
        // GET: CheckOut
        //OrderVierModel orderViewModel = new OrderVierModel();
        //[HttpPost]
        //public ActionResult AddToShoppingCart(int amount, int bookId)
        //{
        //    Book targetBook = null;
        //    using (BookStoreEntities db=new BookStoreEntities())
        //    {
        //        targetBook = db.Books.SingleOrDefault(b => b.Id == bookId);
        //    }

        //    Customer targetCustomer = null;
        //    if (Session["UserName"]!=null)
        //    {
        //        int userId = Convert.ToInt32(Session["UserId"]);
        //        using (BookStoreEntities db = new BookStoreEntities())
        //        {
        //            targetCustomer = db.Customers.SingleOrDefault(c => c.Id == userId);
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.LoginError = "Login Please!";
        //        return View("Error");
        //    }

        //    Order order = new Order();
        //    using (BookStoreEntities db=new BookStoreEntities())
        //    {
        //        if (db.Orders.Count()==0)
        //        {
        //            order.Id = 1;
        //        }
        //        else
        //        {
        //            order.Id = db.Orders.Max(o => o.Id) + 1;
        //        }
        //    }
        //    //order.Id = (new Random()).Next(1000, 9999);
        //    order.BookId = targetBook.Id;
        //    order.CustomerId = targetCustomer.Id;
        //    order.Amount = amount;
        //    order.Total= targetBook.Price * order.Amount;

        //    orderViewModel.Order = order;
        //    orderViewModel.Book = targetBook;
        //    orderViewModel.Customer = targetCustomer;

        //    return View(orderViewModel);
        //}
        public ActionResult CheckOutBook()
        {
            Order targetOrder = new Order();
            var r = new Random();
            targetOrder.OrderId = r.Next(100, 1000);
            ViewBag.orderId = targetOrder.OrderId;

            Customer targetCustomer = null;
            if (Session["UserName"] != null)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                using (BookStoreEntities db = new BookStoreEntities())
                {
                    targetCustomer = db.Customers.SingleOrDefault(c => c.Id == userId);
                    targetOrder.CustomerId = targetCustomer.Id;
                    ViewBag.OrderAddress = string.Format(targetCustomer.Address1 + ", " + targetCustomer.City + ", " + targetCustomer.State + ", " + targetCustomer.PostCode);
                }
            }
            else
            {
                return View("Error");
            }

            List<Item> cart = (List<Item>)Session["cart"];
            using (BookStoreEntities db = new BookStoreEntities())
            {
                double total = 0;
                for (int i = 0; i < cart.Count(); i++)
                {
                    Book targetBook = cart[i].Book;
                    if (targetBook.UnitsInStock >= cart[i].Amount)
                    {
                        targetBook.UnitsInStock = targetBook.UnitsInStock - cart[i].Amount;
                        db.Entry(targetBook).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        if (db.Orders.Count() == 0)
                        {
                            targetOrder.Id = 1;
                        }
                        else
                        {
                            targetOrder.Id = db.Orders.Max(o => o.Id) + 1;
                        }
                        targetOrder.Amount = cart[i].Amount;
                        targetOrder.Total = targetBook.Price * cart[i].Amount;
                        targetOrder.BookId = targetBook.Id;
                        db.Entry(targetOrder).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Low stocks";
                        return View("Error");
                    }
                    total += targetOrder.Total;
                }
            }

            try
            {
                MailMessage mail = null;
                string thanksMessage = "This is an automatically generated message to confirm receipt of your order via the Internet. You do not need to reply to this e-mail, but you may wish to save it for your records.Your order should arrive in four to six weeks. Thank you.";
                if (ViewBag.Message == "Low stocks")
                {
                    mail = new MailMessage("xiamiliu@gmail.com", targetCustomer.EmailAddress, "Order Error", "Sorry, this order is not placed!");
                }
                else
                {
                    mail = new MailMessage("xiamiliu@gmail.com", targetCustomer.EmailAddress, "Thanks for your order!", thanksMessage);
                }

                mail.From = new MailAddress("xiamiliu@gmail.com", "Xiami");
                mail.IsBodyHtml = false; // necessary if you're using html email

                NetworkCredential credential = new NetworkCredential("xiamiliu@gmail.com", "lgx_9719014");
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Send(mail);
                ViewBag.Email = targetCustomer.EmailAddress;
            }
            catch (Exception)
            {
                ViewBag.Error = "Sorry, we couldn't send the email to confirm your RSVP.";
                return View("ThanksOrder");
            }
            return View("ThanksOrder", cart);
        }
    }
}