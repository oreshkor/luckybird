using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace luckybird
{
    public class SoccerChamp
    {
        readonly string _headTitle;
        readonly EventLine[] _lines;

        internal SoccerChamp(string headTitle, IEnumerable<EventLine> lines)
        {
            Guard.Against(string.IsNullOrWhiteSpace(headTitle), "headTitle");
            Guard.Against(lines == null, "lines");

            _headTitle = headTitle;
            _lines = lines.ToArray();

            Guard.Against(_lines.Length == 0, "lines", "Parsed champ has no lines.");
        }

        public string Title
        {
            get { return _headTitle; }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("{Division: {0}. Lines count: {1}.", _headTitle, _lines.Length);
            builder.AppendLine();
            foreach (var line in _lines)
            {
                builder.AppendLine(line.ToString());
            }
            return builder.ToString();
        }

    }
}
