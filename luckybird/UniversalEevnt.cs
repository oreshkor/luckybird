using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace luckybird
{
    public class EventName
    {
        readonly string Period;
        readonly string Event;
        readonly double Details;

        public EventName(string period, string @event, double details)
        {
           
            Period = period;
            Event = @event;
            Details = details;
        }
    }




    public struct Coefficient
    {
        readonly double _value;

        public Coefficient(double coefficient)
            : this()
        {
            Guard.Against(coefficient <= 0, "coefficient", "Coefficient must be greater then zero.");

            _value = coefficient;
        }

        public double Inverted()
        {
            return 1 / _value;
        }
    }

    public class SiteId
    {

    }

    public sealed class DiscoveredLine
    {
        public readonly SiteId FoundOn;
        public readonly DateTimeOffset DiscoveredAt;//TODO instroduce Version class
        public readonly Line Line;

        public DiscoveredEvent(SiteId siteId, DateTimeOffset discoveredAt, Line line)
        {
            Guard.Against(ReferenceEquals(siteId, null), "siteId");
            Guard.Against(discoveredAt == DateTimeOffset.MinValue, "dicoveredAt");
            Guard.Against(ReferenceEquals(line, null), "line");

            FoundOn = siteId;
            DiscoveredAt = discoveredAt;
            Line = line;
        }
    }
    public class Line
    {
        public readonly LineId Id;
        public readonly IEnumerable<Event> Events;

        public Line(LineId id, IEnumerable<Event> events)
        {
            Id = id;
            Events = events.ToArray();
        }

        public class LineId
        {
            public LineId(string sport, string league, string homeTeam, string awayTeam, DateTimeOffset startDate)
                :this()
            {
                Sport = sport;
                League = league;
                HomeTeam = homeTeam;
                AwayTeam = awayTeam;
                HappensAt = HappensAt;
            }
            public readonly string Sport;
            public readonly string League;
            public readonly string HomeTeam;
            public readonly string AwayTeam;
            public readonly DateTimeOffset HappensAt;

        }
    



    public class Event
    {
        public readonly Coefficient Coefficient;
        public readonly EventName Name;

        public Event(EventName name, Coefficient coefficient)
        {
            Coefficient = coefficient;
            Name = name;
        }

    }


    public class NewEventBuilder
    {
        readonly DateTimeOffset _constractedAt;
        SiteId _siteId;
        string _league;
        string _sport;
        string _homeTeam;
        string _awayTeam;
        string _period;
        string _event;
        double _details;
        Coefficient _coefficient;
        private DateTimeOffset _when;

        public NewEventBuilder(DateTimeOffset constractedAt)
        {
            this._constractedAt = constractedAt;
        }


        internal NewEventBuilder ForSite(SiteId siteId)
        {
            _siteId = siteId;
            return this;
        }

        internal NewEventBuilder ForSportKind(string p)
        {
            _sport = p;
            return this;
        }

        internal NewEventBuilder ForLeague(string p)
        {
            _league = p;
            return this;
        }

        internal NewEventBuilder Paticipance(string p1, string p2)
        {
            _homeTeam = p1;
            _awayTeam = p2;
            return this;
        }

        internal NewEventBuilder HappensAt(DateTimeOffset when)
        {
            _when = when;
            return this;
        }

        internal NewEventBuilder Period(string p)
        {
            _period = p;
            return this;
        }

        internal NewEventBuilder Event(string p)
        {
            _event = p;
            return this;
        }

        internal NewEventBuilder WithDetails(double p)
        {
            _details = p;
            return this;
        }

        internal NewEventBuilder Coefficient(double p)
        {
            _coefficient = new Coefficient(p);
            return this;
        }

        internal DiscoveredEvent Build()
        {
            return new DiscoveredEvent(_siteId, _constractedAt, new Event(new EventName(_sport, _league, _homeTeam, _awayTeam, _when, _period, _event, _details), _coefficient));
        }
    }

    public static class ForkFinder : IHandler<LineHasBeenChanged>
    {
        public static bool Handle(LineHasBeenChanged changedLineEvent)
        {
            var changedLine = changedLineEvent.ChandgedLine;
            var otherLines = changedLineEvent.LineBuckle;




            return ets.Select(e => e.Coefficient.Inverted()).Sum() > 1;
        }

        private void FindFork( params Line[] lines )
        {
            var groups = lines.SelectMany( line => line.Events )
                              .GroupBy( e => e.Name );

            foreach( var group in groups)
            {
                var forks = PairComaprison(group.AsCombinator()).ToArray();


            }
        }

private IEnumerable<Tuple<Event,Event>  PairComaprison(IEnumerable<Tuple<Event,Event>> sequence)
{
 	return sequence.Where( (e1) => e1.Item1.Coefficient.Inverted() + e1.Item2.Coefficient.Inverted() > 1 )
}
    }

    public class LineTracking : IHandler<DiscoveredLine>//, IPublisher<LineHasBeenChanged>
    {
        class MultiSiteLine
        {
            IDictionary<LineId, IDictionary<SiteId,Line>> _lineStorage= new Dictionary<LineId,IDictionary<SiteId,Line>>();

            public void AddLatestVersionOfLineForSite(SiteId foundOn, Line line)
            {
                IDictionary<SiteId,Line> siteBuckle;

                if(!_lineStorage.TryGetValue(line.Id, out siteBuckle )){
                    siteBuckle = new Dictionary<SiteId, Line>();
                    _lineStorage[line.Id] = siteBuckle;
                }

                siteBuckle[siteId] = line;
            }
        }

        readonly IDictionary<EventName, List<DiscoveredEvent>> _events = new Dictionary<string, List<Event>>();

        public void Handle(DiscoveredLine eventData)
        {
           _lineStorage.AddLatestVersionOfLineForSite(eventData.FoundOn, eventData.Line)
        }
    }

    [TestFixture]
    public class fork_finder_should
    {
        [Test]
        public void identify_a_fork()
        {
            var builder = CreateBuilder();
            var event1 = builder.Coefficient(1.92).Build();
            var event2 = builder.Coefficient(2).Build();

            var isForkFound = ForkFinder.IsForkExists(event1, event2);

            Assert.IsTrue(isForkFound);
        }

        private static NewEventBuilder CreateBuilder()
        {
            var eventBuilder = new NewEventBuilder(DateTimeOffset.UtcNow);

            return eventBuilder.ForSite(new SiteId())
                            .ForSportKind("Soccer")
                            .ForLeague("England Premier")
                            .Paticipance("Team 1", "Team2")
                            .HappensAt(DateTimeOffset.UtcNow)
                            .Period("Game")
                            .Event("Total")
                            .WithDetails(0.25);
        }
    }
}
