using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace luckybird
{
    [TestFixture]
    public class Champ_title_parsing_should
    {
        static IEnumerable<SoccerChamp> GetParsedChamps(string response)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(response));
            var parser = new SoccerPageParser();

            return parser.ParsePage(stream).ToList();
        }

        [Test]
        public void find_and_parse_champ_title()
        {
            var responsebody = @"<div class='main-block-events'>
                                    <div class='block-events-head'>
                                        Champ description place
                                    </div>
                                    <div class='foot-market-border'>
                                         <table class='foot-market'> 
                                            <tbody><tr><th class='coupone'><div class='markets-hint'>Event type</div></th></tr></tbody>
                                            <tbody><tr class='event-header'>
                                                        <td class='first'>
                                                            <table><tbody><tr>
                                                                <td class='today-name'><span class='command'>
                                                                    <div class='today-member-name'>Command 1</div>
                                                                    <div class='today-member-name'>Command 2</div>
                                                                </span></td>
                                                                <td class='date'>15:00</td>
                                                            </tr></tbody></table>
                                                        </td>
                                                        <td class='js-price'>
                                                            <span class='selection-link'>1111.1111</span>
                                                        </td>
                                                    </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>";

            var champ = GetParsedChamps(responsebody).FirstOrDefault();

            Assert.NotNull(champ);
            Assert.AreEqual("Champ description place", champ.Title);

            var line = champ.Lines.FirstOrDefault();

            Assert.NotNull(line);
            Assert.AreEqual("Command 1-Command 2", line.Desciption.What);
            Assert.AreEqual(DateTimeOffset.Parse("15:00"), line.Desciption.When);

            var @event = line.Events.FirstOrDefault();

            Assert.AreEqual(1111.1111, @event.Coefficient);
            Assert.AreEqual(string.Empty,  @event.Specification);
            Assert.AreEqual("Event type", @event.Type);
        }

        [Test]
        public void tollerate_inner_nodes_with_text_and_skip_them()
        {
            var responsebody = @"<div class='main-block-events'>
                                    <div class='block-events-head'>
                                        <a>Trash1</a>
                                        Champ description place
                                        <span>Trahs2</span>
                                    </div>
                                    <div class='foot-market-border'>
                                         <table class='foot-market'> 
                                            <tbody><tr><th class='coupone'><div class='markets-hint'>Event type</div></th></tr></tbody>
                                            <tbody><tr class='event-header'>
                                                        <td class='first'>
                                                            <table><tbody><tr>
                                                                <td class='today-name'><span class='command'>
                                                                    <div class='today-member-name'>Command 1</div>
                                                                    <div class='today-member-name'>Command 2</div>
                                                                </span></td>
                                                                <td class='date'>15:00</td>
                                                            </tr></tbody></table>
                                                        </td>
                                                        <td class='js-price'>
                                                            <span class='selection-link'>1111.1111</span>
                                                        </td>
                                                    </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>";

            var champ = GetParsedChamps(responsebody).FirstOrDefault();

            Assert.NotNull(champ);
            Assert.AreEqual("Champ description place", champ.Title);

            var line = champ.Lines.FirstOrDefault();

            Assert.NotNull(line);
            Assert.AreEqual("Command 1-Command 2", line.Desciption.What);
            Assert.AreEqual(DateTimeOffset.Parse("15:00"), line.Desciption.When);

            var @event = line.Events.FirstOrDefault();

            Assert.AreEqual(1111.1111, @event.Coefficient);
            Assert.AreEqual(string.Empty,  @event.Specification);
            Assert.AreEqual("Event type", @event.Type);
        }
    }
}