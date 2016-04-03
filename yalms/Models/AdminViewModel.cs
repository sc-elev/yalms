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

        public int SelectedTeacher { set; get; }
        public IList<SelectListItem> Teachers { set; get; }

        public int SelectedCourse { set; get; }
        public IList<SelectListItem> Courses { set; get; }

        public Dictionary<string, string> StudentsByIndex { get; set; }

        public AdminViewModel()
        {
            UnregisteredUsers = new List<SelectListItem>();
            RegisteredUsers = new List<SelectListItem>();
            Classes = new List<SelectListItem> ();
            Teachers = new List<SelectListItem>();
            Courses = new List<SelectListItem>();
            Role = "Elev";
            StudentsByIndex = new Dictionary<string, string>();
        }
    }  
}