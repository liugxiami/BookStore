using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.Models
{
    public class Item
    {
        private Book book = new Book();
        public Book Book
        {
            get { return book; }
            set { book = value; }
        }

        private int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public Item()
        {

        }
        public Item(Book book, int amount)
        {
            this.book = book;
            this.amount = amount;
        }
    }
}