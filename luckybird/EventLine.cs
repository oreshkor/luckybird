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

        public bool IsSame(EventLine line)
        {
            //TODO improve this code. Making appropriate "equals" methods in classes EventDescriptor & Event
            Guard.Against(line == null, "line");

            if (_events.Count() != line._events.Count())
                return false;

            for( var i = 0; i < _events.Length; ++i)
            {
                var l = _events[i];
                var r = line._events[i];
                if( l.Coefficient != r.Coefficient ||
                    l.Specification != r.Specification || l.Type != r.Type)
                    return false;
            }

            return _eventDescriptor.ToString() == line._eventDescriptor.ToString();
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