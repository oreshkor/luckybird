using System;

namespace luckybird
{
    public class EventId : IEquatable<EventId>
    {
        public readonly string Sport;
        public readonly string League;
        public readonly string HomeTeam;
        public readonly string AwayTeam;
        public readonly DateTimeOffset StartDate;
        public readonly string Period;
        public readonly string BetName;
        public readonly string BetSpecifiaction;


        public bool Equals(EventId other)
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

    public class Bet
    {
        public readonly DateTimeOffset DicoveredAt;
        public readonly EventId Event;
        public readonly Coefficient Coefficient;

        public Bet(DateTimeOffset dicoveredAt, Coefficient coefficient, EventId @event)
        {
            DicoveredAt = dicoveredAt;
            Coefficient = coefficient;
            Event = @event;
        }
    }
}
