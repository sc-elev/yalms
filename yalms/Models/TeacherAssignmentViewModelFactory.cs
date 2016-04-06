using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yalms.Models
{
    public class TeacherAssignmentViewModelFactory
    {
        private Controller controller;
        private EFContext context;
        private IUserProvider userProvider;

        public TeacherAssignmentViewModelFactory(Controller controller,
                                           EFContext context,
                                           IUserProvider user)
        {
            this.controller = controller;
            this.context = context;
            this.userProvider = user;
        }

        public TeacherAssignmentViewModel Create()
        {
            var model = new TeacherAssignmentViewModel(userProvider.UserID(), context);

            return model;
        }
    }
}