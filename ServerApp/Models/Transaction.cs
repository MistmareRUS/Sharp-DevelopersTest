using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public DateTime Date { get; set; }
        public double Sum { get; set; }
        public double Balance { get; set; }
    }
}