using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace yalms.CommonFunctions
{
    public static class CustomConversion
    {
        public static int GetWeekFromDate(DateTime date)
        {
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");
            return cultureInfo.Calendar.GetWeekOfYear(
                date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime GetFirstDayOfWeekFromDate(DateTime date)
        {
            //var week = GetWeekFromDate(date);
            var weekStart = DayOfWeek.Monday;
            int diff = date.DayOfWeek - weekStart;
            if (diff < 0) { diff += 7; }

            return date.AddDays(-1 * diff);
        }

        public static string GetWeekDayFromDate(DateTime date)
        {
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");

            return cultureInfo.DateTimeFormat.GetDayName(date.DayOfWeek);
        }
    }
}