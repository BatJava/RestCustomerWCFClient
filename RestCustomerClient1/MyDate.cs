using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestCustomerClient1
{
    public class MyDate {
    public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public MyDate() {}


        public MyDate(int year, int month, int day)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(Year); sb.Append(", ");
            sb.Append(Month); sb.Append(", ");
            sb.Append(Day);
            return sb.ToString();

        }
    }
}
