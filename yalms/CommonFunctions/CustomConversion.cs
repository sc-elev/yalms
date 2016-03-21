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

        public static string GetWeekDayFromDate(DateTime date)
        {
            var cultureInfo = new System.Globalization.CultureInfo("sv-SE");

            return cultureInfo.DateTimeFormat.GetDayName(date.DayOfWeek);
        }
    }
}