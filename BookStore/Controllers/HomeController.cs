using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? pageNumber)
        {
            int bookPerPage = 8;
            int realPage = pageNumber ?? 1;
            BookStoreEntities dbContext = new BookStoreEntities();
            var maxPage = dbContext.Books.Count() / bookPerPage + (dbContext.Books.Count() % bookPerPage > 0 ? 1 : 0);
            if (realPage > maxPage)
            {
                realPage = 1;
            }
            if (realPage == 0)
            {
                realPage = maxPage;
            }
            List<Book> allbooks = null;
            allbooks = dbContext.Books.OrderBy(b => b.Id).Skip(bookPerPage * (realPage - 1)).Take(bookPerPage).ToList();
            this.ViewBag.totalPage = maxPage;
            this.ViewBag.pageNumber = realPage;
            return View(allbooks);
        }

        [HttpGet]
        public ActionResult BookDetails(int id)
        {
            Book targetBook = null;
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(b => b.Id == id);
            }
            ViewBag.buyingBookId = id;
            return View(targetBook);
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }

    }
}