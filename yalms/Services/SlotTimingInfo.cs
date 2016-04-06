using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace yalms.Services
{

    public class TimingInfo
    {
        public DateTime start;
        public DateTime end;

        public TimingInfo(string from, string to)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            start = DateTime.ParseExact(from, "HH:mm", provider);
            end = DateTime.ParseExact(to, "HH:mm", provider);
        }

        public string FormatFrom(string format = "HH:mm")
        {
            return start.ToString(format);
        }

        public string FormatTo(string format = "HH:mm")
        {
            return end.ToString(format);
        }
    }


    public class SlotTimingInfo
    {
      
        public static IList<TimingInfo> Timings = new List<TimingInfo>() {
            new TimingInfo("08:00", "08:50"),
            new TimingInfo("09:00", "09:50"),
            new TimingInfo("10:00", "10:50"),
            new TimingInfo("11:00", "11:50"),
            new TimingInfo("12:00", "12:50"),
            new TimingInfo("13:00", "13:50"),
            new TimingInfo("14:00", "14:50"),
            new TimingInfo("15:00", "16:50"),
            new TimingInfo("17:00", "17:50"),
        };

        public int LastSlot() 
        { 
            return Timings.Count > 0 ? Timings.Count - 1 : 0; 
        }
      
        public DateTime startTime(int slot)
        {
            return slot > LastSlot() ? default(DateTime) : Timings[slot].start;
        }

        public DateTime endTime(int slot)
        {
            return slot > LastSlot() ? default(DateTime) : Timings[slot].end;
        }
    }
}