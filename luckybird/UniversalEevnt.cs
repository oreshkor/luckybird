using System;

namespace luckybird
{
    public class EventName : IEquatable<EventName>
    {
        public readonly string Sport;
        public readonly string League;
        public readonly string HomeTeam;
        public readonly string AwayTeam;
        public readonly DateTimeOffset StartDate;
        public readonly string Period;
        public readonly string BetName;
        public readonly string BetSpecifiaction;


        public bool Equals(EventName other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return (Sport == other.Sport)
                   && (League == other.League)
                   && (HomeTeam == other.HomeTeam)
                   && (AwayTeam == other.AwayTeam)
                   && (StartDate == other.StartDate)
                   && (Period == other.Period)
                   && (BetName == other.BetName)
                   && (BetSpecifiaction == other.BetSpecifiaction);
        }
    }
    

    

    public struct Coefficient 
    {
        readonly double _value;

        public Coefficient(double value)
            : this()
        {
            _value = value;
        }
    }
    
    public class SiteId
    {
        
    }

    public class Event
    {
        readonly SiteId SiteId;
        readonly DateTimeOffset DicoveredAt;
        readonly Coefficient Coefficient;
        readonly EventName _name;

        public Event(DateTimeOffset dicoveredAt, Coefficient coefficient, EventName name, SiteId siteId)
        {
            DicoveredAt = dicoveredAt;
            Coefficient = coefficient;
            _name = name;
            SiteId = siteId;
        }

    }

    
}
