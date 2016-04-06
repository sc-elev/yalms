using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yalms.Models
{
    public class TeacherScheduleViewModelFactory
    {
        private Controller controller;
        private EFContext context;
        private IUserProvider userProvider;

        public TeacherScheduleViewModelFactory(Controller controller,
                                           EFContext context,
                                           IUserProvider user)
        {
            this.controller = controller;
            this.context = context;
            this.userProvider = user;
        }

        public TeacherScheduleViewModel Create(DateTime? date)
        {
            var teacher_UserID = userProvider.UserID();
            var scheduleDate = date == null ? new DateProvider().Today().Date : (DateTime)date;

            var model = new TeacherScheduleViewModel(scheduleDate, teacher_UserID, context);

            return model;
        }
    }
}