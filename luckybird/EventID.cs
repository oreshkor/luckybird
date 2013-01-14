
namespace luckybird.Soccer
{
    public class EventID
    {
        //public Sport readonly Sport;
        //public League readonly League;
        //public Team readonly HomeTeam;
        //public Team readonly AwayTeam;
        //public DateTimeOffset readonly When;
    }

    public struct Coefficient
    {
        private readonly double _value;

        public Coefficient(double value)
            : this()
        {
            _value = value;
        }
    }

    public struct SpreadValue
    {
        private readonly double _value;

        public SpreadValue(double value)
            : this()
        {
            _value = value;
        }
    }

    public class Spread
    {
        public Team Winner;
        public readonly SpreadValue Spread;
        public readonly Coefficient Price;
    }


}
