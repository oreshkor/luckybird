using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace luckybird
{
    public class MaraphoneSoccerPageBuilder
    {
        string _champtTitle = "default champ title";
        List<string> _events = new List<string>();
        string _eventTypes;

        public MaraphoneSoccerPageBuilder WithChamp(string title)
        {
            _champtTitle = title;
            return this;
        }
        public MaraphoneSoccerPageBuilder WithTodayLine(string command1, string command2, params double[] coefficient)
        {
            _events.Add(CreateTodayLine(command1, command2, coefficient));
            return this;
        }

        string CreateTodayLine(string command1, string command2, double[] coefficient)
        {
            const string template = @"<tr class='event-header'>
                                                        <td class='first'>
                                                            <table><tbody><tr>
                                                                <td class='today-name'><span class='command'>
                                                                    <div class='today-member-name'>{0}</div>
                                                                    <div class='today-member-name'>{1}</div>
                                                                </span></td>
                                                                <td class='date'>15:00</td>
                                                                {2}
                                                            </tr></tbody></table>
                                                        </td></tr>";

            const string coeffTemplate = @"<td class='js-price'><span class='selection-link'>{0}</span></td>";

            var renderedCoefficients = ConactenatePeaces(coefficient.Select(d => string.Format(coeffTemplate, d)));

            return string.Format(template, command1, command2, renderedCoefficients);
        }

        public MaraphoneSoccerPageBuilder ExpectedEvents(params string[] eventtypes)
        {
            _eventTypes = FormatEventTypes(eventtypes);

            return this;
        }

        private string FormatEventTypes(string[] eventtypes)
        {
            const string template = "<th class='coupone'><div class='markets-hint'>{0}</div></th>";

            return ConactenatePeaces(eventtypes.Select(e => string.Format(template, e)));
        }

        static string ConactenatePeaces(IEnumerable<string> peaces)
        {
            return string.Join("", peaces.ToArray());

        }

        public SoccerChamp Build()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(BuildResponce()));
            var parser = new SoccerPageParser();

            return parser.ParsePage(stream).First();
        }

        private string BuildResponce()
        {
            if (_events.Count == 0)
                WithTodayLine("Command 1", "Command 2", 1111.1111);
            if (_eventTypes == null)
                ExpectedEvents("default event type");

            return string.Format(@"<div class='main-block-events'>
                                    <div class='block-events-head'>
                                        {0}
                                    </div>
                                    <div class='foot-market-border'>
                                         <table class='foot-market'> 
                                            <tbody><tr>{1}</tr></tbody>
                                            <tbody>
                                                {2}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>",
                                _champtTitle,
                                _eventTypes,
                                ConactenatePeaces(_events)

                         );
        }
    }
    [TestFixture]
    public class Champ_title_parsing_should
    {
        [Test]
        public void find_and_parse_champ_title()
        {
            var pageBuilder = new MaraphoneSoccerPageBuilder().WithChamp("Champ description place");

            var champ = pageBuilder.Build();

            Assert.NotNull(champ);
            Assert.AreEqual("Champ description place", champ.Title);

            var line = champ.Lines.FirstOrDefault();

            Assert.NotNull(line);
            Assert.AreEqual("Command 1-Command 2", line.Desciption.What);
            Assert.AreEqual(DateTimeOffset.Parse("15:00"), line.Desciption.When);

            var @event = line.Events.FirstOrDefault();

            Assert.AreEqual(1111.1111, @event.Coefficient);
            Assert.AreEqual(string.Empty, @event.Specification);
            Assert.AreEqual("default event type", @event.Type);
        }

        [Test]
        public void tollerate_inner_nodes_with_text_and_skip_them()
        {
            var pageBuilder = new MaraphoneSoccerPageBuilder().WithChamp("<a>href text</a> Champ description place <span>span text</span>");

            var champ = pageBuilder.Build();

            Assert.NotNull(champ);
            Assert.AreEqual("Champ description place", champ.Title);
        }
    }

    [TestFixture]
    public class Line_parsing_should
    {
        [Test]
        public void identify_today_lines_with_event()
        {
            var pageBuilder = new MaraphoneSoccerPageBuilder()
                                    .ExpectedEvents("1", "2", "3", "4")
                                    .WithTodayLine("c1", "c2", 1, 2, 3, 4);

            var champ = pageBuilder.Build();

            var line = champ.Lines.First();

            var expectedLine = new EventLine("c1-c2", DateTimeOffset.Parse("15:00"),
                new[] 
                {
                    new MaraphoneEvent("1", 1),
                    new MaraphoneEvent("2", 2),
                    new MaraphoneEvent("3", 3),
                    new MaraphoneEvent("4", 4),
                });


            Assert.IsTrue(line.IsSame(expectedLine), "actual line is differed from expected" );
        }
    }
}