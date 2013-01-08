using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace luckybird
{
    public class EventLine
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

        public EventDescriptor Desciption
        {
            get { return _eventDescriptor; }
        }

        public IEnumerable<Event> Events
        {
            get { return _events; }
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
}