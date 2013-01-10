using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace luckybird
{
    public class PinnacleFeedParser
    {
        public IEnumerable<FootballEvent> Parse(string file)
        {
            var xmlPathDoc = new XPathDocument(file);

            return NavigateSprts(xmlPathDoc.CreateNavigator());
        }

        static IEnumerable<FootballEvent> NavigateSprts(XPathNavigator nav)
        {
            // move to the root and the first element - <books>
            nav.MoveToRoot();


            var sports = nav.Select("//rsp/fd/sports/sport");
            while (sports.MoveNext())
            {
                var sport = sports.Current;
                var sportid = GetIdFromNode(sport);
                var leagues = sport.Select("leagues/league");

                while (leagues.MoveNext())
                {
                    var leagueid = GetIdFromNode(leagues.Current);
                    var events = leagues.Current.Select("events/event");
                    while (events.MoveNext())
                    {
                        var @event = events.Current;
                        var eventid = GetIdFromNode(@event);

                        var when = DateTimeOffset.Parse(GetChildNodeValue(@event, "startDateTime").First());
                        var homeTeam = GetChildNodeValue(@event, "homeTeam/name").First();
                        var awayTeam = GetChildNodeValue(@event, "awayTeam/name").First();

                        var periods = @event.Select("periods/period");
                        while (periods.MoveNext())
                        {
                            var period = periods.Current;

                            var name = GetChildNodeValue(period, "description").First();
                            var spreads = period.Select("spreads/spread");

                            while (spreads.MoveNext())
                            {
                                var spread = spreads.Current;
                                var awaySpread = double.Parse(GetChildNodeValue(spread, "awaySpread").First());
                                var awayPrice = double.Parse(GetChildNodeValue(spread, "awayPrice").First());
                                var homeSpread = double.Parse(GetChildNodeValue(spread, "homeSpread").First());
                                var homePrice = double.Parse(GetChildNodeValue(spread, "homePrice").First());
                            }

                            var totals = period.Select("totals/total");

                            while (totals.MoveNext())
                            {
                                var total = totals.Current;

                                var points = double.Parse(GetChildNodeValue(total, "points").First());
                                var overPrice = double.Parse(GetChildNodeValue(total, "overPrice").First());
                                var underPrice = double.Parse(GetChildNodeValue(total, "underPrice").First());
                            }
                        }
                    }
                }
            }
            return new FootballEvent[0];


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
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTimeOffset When { get; set; }
        public IEnumerable<Period> Periods { get; set; }
    }

    public class Period
    {
        public DateTimeOffset Cutoff { get; set; }
        public byte Number { get; set; }
        public IEnumerable<Spread> Spreads { get; set; }
        public IEnumerable<Total> Totals { get; set; }

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
        //{sport}{league}{home}{away}{when}{event}

        [Test]
        public void identify_leages_content()
        {
            var parser = new PinnacleFeedParser();

            parser.Parse(@"D:\projects\luckybird\luckybird\luckybird\pinnacle.soccer.oneevent.xml");




            //var football = pinnacleFeed.Sports
            //                    .Where(s => s.id == 29)
            //                    .SelectMany(s => s.Leagues)
            //                    .Where(l => l.id == 2242)
            //                    .SelectMany(l => l.Events).First();



        }
    }
}

