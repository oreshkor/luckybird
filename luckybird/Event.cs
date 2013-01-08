namespace luckybird
{
    class Event
    {
        readonly string _type;
        readonly double _coefficient;
        readonly string _specification;

        public Event(string type, double coefficient, string specification = null)
        {
            Guard.Against(string.IsNullOrWhiteSpace(type), "type");
            

            this._type = type;
            this._coefficient = coefficient;
            this._specification = specification;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_specification))
                return string.Format("{0}:{1}", _type, _coefficient);
            return string.Format("{0}-{1}:{2}", _type, _specification, _coefficient);
        }
    }
}
