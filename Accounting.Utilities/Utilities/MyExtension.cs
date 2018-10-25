using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Accounting.Utilities.Utilities
{
    public static class MyExtension
    {
        public static string ToShamsi(this DateTime miladi)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(miladi) + "/" + pc.GetMonth(miladi) + "/" + pc.GetDayOfMonth(miladi) + "  " + pc.GetHour(miladi) + ":" + pc.GetMonth(miladi) + ":" + pc.GetSecond(miladi);
        }
        public static DateTime ToMiladi(this DateTime ShamsiDate)
        {
            return new DateTime(ShamsiDate.Year, ShamsiDate.Month, ShamsiDate.Day, new System.Globalization.PersianCalendar());
        }
    }
}
