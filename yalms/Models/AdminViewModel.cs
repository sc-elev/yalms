using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yalms.Models
{
    public class AdminViewModel
    {
        public int SelectedUser { set; get; }
        public IList<SelectListItem> UnregisteredUsers { set; get; }
        public int SelectedClass { set; get; }
        public IList<SelectListItem> Classes { set; get; }
        public string Role { set; get; }

        public int SelectedVictim { set; get; }
        public IList<SelectListItem> RegisteredUsers { set; get; }

        public AdminViewModel()
        {
            UnregisteredUsers = new List<SelectListItem>();
            RegisteredUsers = new List<SelectListItem>();
            Classes = new List<SelectListItem> ();
            Role = "Elev";
        }
    }  
}