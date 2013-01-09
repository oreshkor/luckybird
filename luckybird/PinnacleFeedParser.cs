using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace luckybird
{
    public class PinnacleFeedParser
    {

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
        public double AwaySpread { get; set; }
        public int AwayPrice { get; set; }
        public double HomeSpread { get; set; }
        public int HomePrice { get; set; }
    }

    public class Total
    {
        public double Points { get; set; }
        public double OverPrice { get; set; }
        public double UnderPrive { get; set; }
    }

    public class MoneyLine
    {
        public int AwayPrice { get; set; }
        public int DrawPrice { get; set; }
        public int HomePrice { get; set; }
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
            //var football = pinnacleFeed.Sports
            //                    .Where(s => s.id == 29)
            //                    .SelectMany(s => s.Leagues)
            //                    .Where(l => l.id == 2242)
            //                    .SelectMany(l => l.Events).First();



        }
    }
}
