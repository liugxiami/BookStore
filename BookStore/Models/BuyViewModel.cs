using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class BuyViewModel
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public int Amount { get; set; }
    }
}