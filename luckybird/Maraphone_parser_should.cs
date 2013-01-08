using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace luckybird
{
    [TestFixture]
    public class Maraphone_parser_should
    {
        static IEnumerable<SoccerChamp> GetParsedChamps()
        {
            var stream = File.OpenRead(Environment.CurrentDirectory + @"\marafon.htm");
            var parser = new SoccerPageParser();

            return parser.ParsePage(stream).ToList();
        }

        [Test]
        public void parse_all_lines_withou_errors()
        {
            var champs = GetParsedChamps();


            Assert.That(1, Is.EqualTo(champs.Count()));
        }

        //[Test]
        //public void parse_table_header()
        //{
        //    var champ = GetParsedChamps();

        //    CollectionAssert.AreEqual(new[] { "Win 1", "Draw", "Win 2", "Win 1 or Draw", "Win 1 or Win 2", "Draw or Win 2", "Handicap1", "Handicap2", "Total under", "Total over" }, );

        //}


        //[Test]
        //public void parse_commands()
        //{
        //    var dom = GetParsedLines();

        //    var tablebets = dom.Select("table.foot-market tr.event-header:first");

        //    Assert.AreEqual(1, tablebets.Length);

        //    var commands = dom.Select("td.today-name > span.command > div.today-member-name", tablebets).Map(node => node.Cq().Text().Trim().Replace(nbsp, ' '));

        //    Assert.AreEqual(2, commands.Count());

        //    CollectionAssert.AreEqual(new[] { "Eupen", "Lommel United" }, commands);

        //    var date = dom.Select("td.date", tablebets).Text().Trim();

        //    CollectionAssert.AreEqual("19:00", date);

        //}

        //[Test]
        //public void parse_bets()
        //{
        //    var dom = GetParsedLines();

        //    var game = dom.Select("table.foot-market tr.event-header:first");

        //    Assert.AreEqual(1, game.Length);

        //    var bets = dom.Select("td.js-price > span.selection-link", game).Map(node => node.Cq().Text().Trim());

        //    CollectionAssert.AreEqual(new[] { "2.20", "3.40", "3.40", "1.333", "1.333", "1.69", "1.60", "2.46", "2.00", "1.88" }, bets);

        //}
    }
}