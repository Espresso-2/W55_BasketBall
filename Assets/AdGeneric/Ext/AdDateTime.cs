using System;
using UnityEngine;

namespace AdGeneric.Ext
{
    [Serializable]
    public class AdDateTime:IFormattable
    {
        [Range(2024,2050)]public int year=DateTime.Now.Year;
        [Range(1,12)]public int month=DateTime.Now.Month;
        [Range(1,31)]public int day=DateTime.Now.Day;

        [Range(0,23)]public int hour=19;
        [Range(0,59)]public int minute=0;
        [Range(0,59)]public int second=0;

        public AdDateTime()
        {
        }

        public AdDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.second = second;
        }

        public static implicit operator DateTime(AdDateTime o) =>
            new DateTime(o.year, o.month, o.day, o.hour, o.minute, o.second);

        public string ToString(string format, IFormatProvider formatProvider) => ((DateTime) this).ToString(format, formatProvider);
    }
}