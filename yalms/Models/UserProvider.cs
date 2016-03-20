using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yalms.Models
{

    public interface IUserProvider
    {
        string Who();
        string Role();
    }


    public class DummyUserProvider: IUserProvider
    {
        private string who;
        private string role;

        public string Who()   { return who;  }

        public string Role()  {  return role;   }

        public DummyUserProvider(string w, string r)
        {
            who = w;
            role = r;
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

        public UserProvider(Controller c) { controller = c; }

    }
}