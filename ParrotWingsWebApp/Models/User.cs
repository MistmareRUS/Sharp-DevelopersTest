using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ParrotWingsWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
    }
}