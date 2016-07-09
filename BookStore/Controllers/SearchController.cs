using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpPost]
        public ActionResult SearchBooksResult(string query)
        {
            List<Book> results = null;
            if (!string.IsNullOrEmpty(query))
            {
                ViewBag.SearchQuery = query;
                BookStoreEntities dbContext = new BookStoreEntities();
                results = dbContext.Books.Where(p => p.Name.Contains(query) || p.Author.Contains(query)).ToList();
                this.ViewBag.booksCount = results.Count();
                return View(results);
            }
            else
            {
                return View("Error");
            }

        }
        [HttpGet]
        public ActionResult BookDetails(int id)
        {
            Book targetBook = null;
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(b => b.Id == id);
            }
            return View(targetBook);
        }
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
    }
}