﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yalms.Models
{
    public interface IDateProvider
    {
        DateTime Today();
    }


    public class DummyDateProvider: IDateProvider
    {
        private DateTime dateTime;

        public DateTime Today() { return dateTime.Date; }

        public DummyDateProvider(string when)
        {
            dateTime = DateTime.Parse(when);
        }

        public DummyDateProvider(DateTime when)
        {
            dateTime = when;
        }
    }


    public class DateProvider : IDateProvider 
    {
        public DateTime Today() { return DateTime.Today; }

        public DateProvider() { }
    }
}