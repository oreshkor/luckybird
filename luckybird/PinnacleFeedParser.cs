using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace luckybird
{
    public class PinnacleFeedParser
    {
        static Func<IEnumerable<string>, double> getFirstAndParseToDouble = e => double.Parse(e.First());

        readonly Func<int, string> _sportKindResonlver;
        readonly Func<int, string> _leagueNameResolver;

        public PinnacleFeedParser(Func<int, string> sportKindResolver, Func<int, string> leagueNameResolver)
        {
            _sportKindResonlver = sportKindResolver;
            _leagueNameResolver = leagueNameResolver;
        }

        public IEnumerable<FootballEvent> Parse(string file)
        {
            var xmlPathDoc = new XPathDocument(file);

            return NavigateSprts(xmlPathDoc.CreateNavigator());
        }

        IEnumerable<FootballEvent> NavigateSprts(XPathNavigator nav)
        {
            // move to the root and the first element - <books>
            nav.MoveToRoot();


            var sports = nav.Select("//rsp/fd/sports/sport");
            while (sports.MoveNext())
            {
                var sport = sports.Current;
                var sportName = _sportKindResonlver(GetIdFromNode(sport));
                var leagues = sport.Select("leagues/league");

                while (leagues.MoveNext())
                {
                    var leagueName = _leagueNameResolver(GetIdFromNode(leagues.Current));
                    var events = leagues.Current.Select("events/event");
                    while (events.MoveNext())
                    {
                        var @event = events.Current;
                        var eventid = GetIdFromNode(@event);

                        var when = DateTimeOffset.Parse(GetChildNodeValue(@event, "startDateTime").First());
                        var homeTeam = GetChildNodeValue(@event, "homeTeam/name").First();
                        var awayTeam = GetChildNodeValue(@event, "awayTeam/name").First();

                        var periods = @event.Select("periods/period");
                        var pperiods = ParsePeriods(periods).ToArray();

                        yield return new FootballEvent { AwayTeam = awayTeam, HomeTeam = homeTeam, League = leagueName, Sport = sportName, Periods = pperiods, When = when };
                    }
                }
            }
        }

        private static IEnumerable<Period> ParsePeriods(XPathNodeIterator periods)
        {

            while (periods.MoveNext())
            {
                var period = periods.Current;

                var name = GetChildNodeValue(period, "description").First();
                var spreads = period.Select("spreads/spread");

                var pperiods = ParseSpreads(spreads).ToArray();

                var totals = period.Select("totals/total");

                var ptotals = ParseTotals(totals).ToArray();

                var moneyLine = ParseMoneyLine(period.Select("moneyLine"));

                yield return new Period { Description = name, Spreads = pperiods, Totals = ptotals, MoneyLine = moneyLine };
            }


        }

        private static MoneyLine ParseMoneyLine(XPathNodeIterator xMoneLine)
        {
            if (!xMoneLine.MoveNext())
                return null;

            var awayPrice = getFirstAndParseToDouble(GetChildNodeValue(xMoneLine.Current, "awayPrice"));
            var homePrice = getFirstAndParseToDouble(GetChildNodeValue(xMoneLine.Current, "homePrice"));
            var drawPrice = getFirstAndParseToDouble(GetChildNodeValue(xMoneLine.Current, "drawPrice"));

            return new MoneyLine { AwayPrice = awayPrice, HomePrice = homePrice, DrawPrice = drawPrice };
        }

        private static IEnumerable<Total> ParseTotals(XPathNodeIterator totals)
        {
            while (totals.MoveNext())
            {
                var total = totals.Current;

                var points = getFirstAndParseToDouble(GetChildNodeValue(total, "points"));
                var overPrice = getFirstAndParseToDouble(GetChildNodeValue(total, "overPrice"));
                var underPrice = getFirstAndParseToDouble(GetChildNodeValue(total, "underPrice"));

                yield return new Total { Points = points, OverPrice = overPrice, UnderPrive = underPrice };
            }
        }

        private static IEnumerable<Spread> ParseSpreads(XPathNodeIterator spreads)
        {
            while (spreads.MoveNext())
            {
                var spread = spreads.Current;
                var awaySpread = getFirstAndParseToDouble(GetChildNodeValue(spread, "awaySpread"));
                var awayPrice = getFirstAndParseToDouble(GetChildNodeValue(spread, "awayPrice"));
                var homeSpread = getFirstAndParseToDouble(GetChildNodeValue(spread, "homeSpread"));
                var homePrice = getFirstAndParseToDouble(GetChildNodeValue(spread, "homePrice"));

                yield return new Spread { AwaySpread = awaySpread, AwayPrice = awayPrice, HomePrice = homePrice, HomeSpread = homeSpread };
            }
        }

        static int GetIdFromNode(XPathNavigator nav)
        {
            var cl = nav.Clone();

            var navlist = cl.Select("id");

            while (navlist.MoveNext())
            {
                return int.Parse(navlist.Current.Value);
            }
            throw new FormatException();
        }

        static IEnumerable<string> GetChildNodeValue(XPathNavigator nav, string path)
        {
            var cl = nav.Clone();

            var navlist = cl.Select(path);

            while (navlist.MoveNext())
            {
                yield return navlist.Current.Value;
            }
        }
    }

    public class FootballEvent
    {
        public string Sport;
        public string League;
        public string HomeTeam;
        public string AwayTeam;
        public DateTimeOffset When;
        public IEnumerable<Period> Periods;
    }

    public class Period
    {
        public DateTimeOffset Cutoff;
        public byte Number;
        public string Description;
        public IEnumerable<Spread> Spreads;
        public IEnumerable<Total> Totals;
        public MoneyLine MoneyLine;

    }
    public class Spread
    {
        public double AwaySpread;
        public double AwayPrice;
        public double HomeSpread;
        public double HomePrice;
    }

    public class Total
    {
        public double Points;
        public double OverPrice;
        public double UnderPrive;
    }

    public class MoneyLine
    {
        public double AwayPrice;
        public double DrawPrice;
        public double HomePrice;
    }
    public class MaxBetAmount
    {
        public int Spread;
        public int TotalPoints;
        public int MoneyLine;
    }
    [TestFixture]
    public class Pinnacle_parser_should
    {
        //{sport}{league}
        //{home}{away}{when}
        //{event}

        [Test]
        public void parse_feed_content()
        {
            var parser = new PinnacleFeedParser(id => "sport" + id, id => "league" + id);

            var events = parser.Parse(@"D:\projects\luckybird\luckybird\luckybird\pinnacle.soccer.oneevent.xml").ToArray();


            foreach (var @event in events)
            {
                Assert.AreEqual("sport29", @event.Sport);
                Assert.AreEqual("league2242", @event.League);
                Assert.AreEqual("AwayTeam", @event.AwayTeam);
                Assert.AreEqual("HomeTeam", @event.HomeTeam);
                Assert.AreEqual(DateTimeOffset.Parse("2013-01-12T01:29:00Z"), @event.When);
                Assert.AreEqual(2, @event.Periods.Count());

                foreach (var period in @event.Periods)
                {
                    foreach (var spread in period.Spreads)
                    {
                        Assert.AreNotEqual(0, spread.AwayPrice);
                        Assert.AreNotEqual(0, spread.AwaySpread);
                        Assert.AreNotEqual(0, spread.HomeSpread);
                        Assert.AreNotEqual(0, spread.AwayPrice);
                    }

                    foreach (var total in period.Totals)
                    {
                        Assert.AreNotEqual(0, total.Points);
                        Assert.AreNotEqual(0, total.OverPrice);
                        Assert.AreNotEqual(0, total.UnderPrive);
                    }

                    if (period.MoneyLine != null)
                    {
                        Assert.AreEqual(2.74, period.MoneyLine.AwayPrice);
                        Assert.AreEqual(2.74, period.MoneyLine.HomePrice);
                        Assert.AreEqual(3.34, period.MoneyLine.DrawPrice);
                    }
                }
            }

        }
    }
}


