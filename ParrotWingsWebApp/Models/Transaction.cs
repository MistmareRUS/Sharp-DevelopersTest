using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParrotWingsWebApp.Models
{
    public class Transaction
    {        
        public int Id { get; set; }        
        public DateTime Date { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public double Sum { get; set; }
        public double SenderBalance { get; set; }
        public double ReceiverBalance { get; set; }
    }
    
}