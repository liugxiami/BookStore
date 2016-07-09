using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private BookStoreEntities db = new BookStoreEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        private int isExisting(int bookId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            for (int i = 0; i < cart.Count(); i++)
            {
                if (cart[i].Book.Id == bookId)
                {
                    return i;
                }
            }
            return -1;
        }
        [HttpPost]
        public ActionResult OrderNow(int amount, int bookId)
        {
            List<Item> cart = new List<Item>();
            if (Session["cart"] == null)
            {
                Book targetBook = db.Books.SingleOrDefault(b => b.Id == bookId);
                cart.Add(new Item(targetBook, amount));
                Session["cart"] = cart;
            }
            else
            {
                cart = (List<Item>)Session["cart"];
                int index = isExisting(bookId);
                if (index == -1)
                {
                    Book targetBook = db.Books.SingleOrDefault(b => b.Id == bookId);
                    cart.Add(new Item(targetBook, amount));
                }
                else
                {
                    cart[index].Amount += amount;
                }

                Session["cart"] = cart;
            }
            return View("cart", cart);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            int index = isExisting(id);
            List<Item> cart = (List<Item>)Session["cart"];
            cart.RemoveAt(index);
            Session["cart"] = cart;
            return View("cart", cart);
        }
    }
}