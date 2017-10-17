using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeZoneHelper
{
    public static class TimeZoneHelper
    {
        public static IHtmlString ToUserTime(this HtmlHelper helper, DateTimeOffset ModelTime, string timeZone)
        {
            var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var newTime = TimeZoneInfo.ConvertTime(ModelTime, timeZoneId);
            string htmlString = newTime.ToString();
            return new HtmlString(htmlString);
        }

        public static IHtmlString ToUserTime(this HtmlHelper helper, DateTimeOffset ModelTime, string timeZone,string ToStringFormat)
        {
            var timeZoneId = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var newTime = TimeZoneInfo.ConvertTime(ModelTime, timeZoneId);
            string htmlString = newTime.ToString(ToStringFormat);
            return new HtmlString(htmlString);
        }
    }
}