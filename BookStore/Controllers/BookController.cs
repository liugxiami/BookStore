using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.IO;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Show(string sortOrder, int? pageNumber, string searchString)
        {
            BookStoreEntities dbContext = new BookStoreEntities();
            ViewBag.NameSort = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.PriceSort = sortOrder == "Price" ? "Price_desc" : "Price";
            var books = from b in dbContext.Books
                        select b;

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    books = books.OrderByDescending(b => b.Name);
                    break;
                case "Name":
                    books = books.OrderBy(b => b.Name);
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                default:
                    books = books.OrderBy(b => b.Id);
                    break;
            }

            int bookPerPage = 10;
            int realPage = pageNumber ?? 1;

            var maxPage = books.Count() / bookPerPage + (books.Count() % bookPerPage > 0 ? 1 : 0);
            if (realPage > maxPage)
            {
                realPage = 1;
            }
            if (realPage == 0)
            {
                realPage = maxPage;
            }

            //List<Book> allbooks = null;
            var allbooks = books.Skip(bookPerPage * (realPage - 1)).Take(bookPerPage);
            this.ViewBag.totalPage = maxPage;
            this.ViewBag.pageNumber = realPage;
            return View(allbooks.ToList());
        }
        [HttpGet]
        public ActionResult UpdateBook(int id)
        {
            Book targetBook = null;
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(b => b.Id == id);
            }
            return View(targetBook);
        }
        [HttpPost]
        public ActionResult UpdateBook(Book book)
        {
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                if (this.Request.Files != null && this.Request.Files.Count > 0 && this.Request.Files[0].ContentLength > 0 && this.Request.Files[0].ContentLength < 1024 * 100)
                {
                    string fileName = Path.GetFileName(this.Request.Files[0].FileName);
                    string filePathOfWebsite = "~/Images/Covers/" + fileName;
                    book.CoverImagePath = filePathOfWebsite;
                    this.Request.Files[0].SaveAs(this.Server.MapPath(filePathOfWebsite));
                }

                dbContext.Entry(book).State = System.Data.Entity.EntityState.Modified;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }
        [HttpGet]
        public ActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBook(Book book)
        {
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                if (dbContext.Books.Count() == 0)
                {
                    book.Id = 1;
                }
                else
                {
                    book.Id = dbContext.Books.Max(b => b.Id) + 1;
                }
                dbContext.Books.Add(book);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }
        [HttpGet]
        public ActionResult DeleteBook(int id)
        {
            Book targetBook = null;
            using (BookStoreEntities dbContext = new BookStoreEntities())
            {
                targetBook = dbContext.Books.SingleOrDefault(b => b.Id == id);
                dbContext.Entry(targetBook).State = System.Data.Entity.EntityState.Deleted;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Show");
        }
    }
}