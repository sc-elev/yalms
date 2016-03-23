using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace yalms.Models
{

    public interface IUserProvider
    {
        string Who();
        string Role();
        int UserID();
    }


    public class DummyUserProvider: IUserProvider
    {
        private string who;
        private string role;
        private int userID;


        public string Who()   { return who;  }

        public string Role()  {  return role;   }

        public int UserID()  {  return userID;   }

        public DummyUserProvider(string w, string r, int u)
        {
            who = w;
            role = r;
            userID = u;
        }
    }


    public class UserProvider: IUserProvider
    {
        private Controller controller;

        public string Role()
        {
            if (controller.User.IsInRole("teacher"))
                return "teacher";
            if (controller.User.IsInRole("student"))
                return "student";
            return "";
        }

        public string Who()  { return controller.User.Identity.Name; }

        public int UserID() { 
            return Convert.ToInt32( controller.User.Identity.GetUserId());
        }

        public UserProvider(Controller c) { controller = c; }

    }
}