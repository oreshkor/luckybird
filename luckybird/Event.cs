namespace luckybird
{
    public class Event
    {
        public readonly string Type;
        public readonly double Coefficient;
        public readonly string Specification;

        public Event(string type, double coefficient, string specification = null)
        {
            Guard.Against(string.IsNullOrWhiteSpace(type), "type");
            

            this.Type = type;
            this.Coefficient = coefficient;
            this.Specification = specification;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Specification))
                return string.Format("{0}:{1}", Type, Coefficient);
            return string.Format("{0}-{1}:{2}", Type, Specification, Coefficient);
        }
    }
}
