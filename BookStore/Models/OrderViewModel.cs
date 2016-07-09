using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class OrderVierModel
    {
        public Order Order { get; set; }
        public Book Book { get; set; }
        public Customer Customer { get; set; }
    }
}