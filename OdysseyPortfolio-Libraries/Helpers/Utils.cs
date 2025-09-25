using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyPortfolio_Libraries.Helpers
{
    public class Utils
    {
        public static string GetTimeAgo(DateTime from, DateTime to)
        {
            var ts = to - from;

            if (ts.TotalSeconds < 60)
                return $"{ts.Seconds} seconds ago";
            if (ts.TotalMinutes < 60)
                return $"{ts.Minutes} minute{(ts.Minutes > 1 ? "s" : "")} ago";
            if (ts.TotalHours < 24)
                return $"{ts.Hours} hour{(ts.Hours > 1 ? "s" : "")} ago";
            if (ts.TotalDays < 30)
                return $"{ts.Days} day{(ts.Days > 1 ? "s" : "")} ago";
            if (ts.TotalDays < 365)
                return $"{(int)(ts.TotalDays / 30)} month{((int)(ts.TotalDays / 30) > 1 ? "s" : "")} ago";

            return $"{(int)(ts.TotalDays / 365)} year{((int)(ts.TotalDays / 365) > 1 ? "s" : "")} ago";
        }

    }
}
