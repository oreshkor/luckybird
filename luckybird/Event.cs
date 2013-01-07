using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace luckybird
{

    class EventDescriptor
    {
        readonly string _title;
        readonly DateTimeOffset _date;

        public EventDescriptor(DateTimeOffset date, string title)
        {
            this._title = title;
            this._date = date;
        }

        public override string ToString()
        {
            return string.Format("{0} in {1}", _title, _date);
        }
    }

    class EventLine
    {
        readonly EventDescriptor _eventDescriptor;
        readonly Event[] _events;

        public EventLine(string title, DateTimeOffset whenHappen, IEnumerable<Event> events)
        {
            Guard.Against(string.IsNullOrWhiteSpace(title), "title");
            Guard.Against(whenHappen == DateTimeOffset.MinValue, "whenHappen", "DateTime is not specified.");
            Guard.Against(events == null, "events", "Line has no events.");

            _eventDescriptor = new EventDescriptor(whenHappen, title);
            _events = events.ToArray();

            Guard.Against(_events.Length == 0, "events", "Line has zero event count.");
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(_eventDescriptor.ToString());
            builder.Append(":");
            builder.AppendLine(string.Join("/", _events.Select(e => e.ToString())));
            return builder.ToString();
        }
    }

    class Event
    {
        readonly string type;
        readonly double coefficient;
        readonly string specification;

        public Event(string type, double coefficient, string specification = null)
        {
            Guard.Against(string.IsNullOrWhiteSpace(type), "type");
            

            this.type = type;
            this.coefficient = coefficient;
            this.specification = specification;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(specification))
                return string.Format("{0}:{2}", type, coefficient);
            return string.Format("{0}-{1}:{2}", type, specification, coefficient);
        }
    }
}
