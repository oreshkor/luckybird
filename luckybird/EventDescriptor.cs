using System;

namespace luckybird
{
    public class EventDescriptor
    {
        readonly string _title;
        readonly DateTimeOffset _date;

        public EventDescriptor(DateTimeOffset date, string title)
        {
            _title = title;
            _date = date;
        }

        public string What
        {
            get { return _title; }
        }

        public DateTimeOffset When
        {
            get { return _date; }
        }

        public override string ToString()
        {
            return string.Format("{0} in {1}", _title, _date);
        }
    }
}