using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yalms.DAL;

namespace yalms.Models
{
    public class StudentMainViewModelFactory
    {
        private Controller controller;
        private EFContext context;
        private IUserProvider userProvider;

        public StudentMainViewModelFactory(Controller controller,
                                           EFContext context,
                                           IUserProvider user)
        {
            this.controller = controller;
            this.context = context;
            this.userProvider = user;
        }


        public StudentMainViewModel Create(IDateProvider date, int DaysToAdd = 0)
        {
            StudentMainViewModel model =
                 controller.TempData["studentViewModel"] as StudentMainViewModel;

            var nextDate = date != null ? date.Today() : DateTime.Now.Date;
            if (model != null && date == null)
            {
                nextDate = model.Today;
            }
            nextDate = nextDate.AddDays(DaysToAdd);
            var nextDay = new DummyDateProvider(nextDate);
            model =
                new StudentMainViewModel(context, userProvider, nextDay);
            controller.TempData["studentViewModel"] = model;

            return model;
        }
    }
}