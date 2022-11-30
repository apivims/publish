using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utility
{
   public class DateTimeUtil 
    {

        public static String GetSQLDateTime(DateTime dateTime)
        {

            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static String GetSQLDate(DateTime dateTime)
        {

            return dateTime.ToString("yyyy-MM-dd");
        }

        public static String GetSQLTime(DateTime dateTime)
        {

            return dateTime.ToString("HH:mm:ss");
        }
    }
}
