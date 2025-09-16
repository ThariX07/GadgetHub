using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadgetHub.Models
{
    public class Distributor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}