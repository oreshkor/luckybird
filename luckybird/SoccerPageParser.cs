using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsQuery;

namespace luckybird
{
    public class SoccerPageParser
    {
        const char Nbsp = '\xa0';
        const char ValidSpace = ' ';

        public IEnumerable<SoccerChamp> ParsePage(Stream response)
        {
            var dom = CreateDomParser(response);
            var heads = dom.Select("div.main-block-events");

            return ParseHead(heads);
        }

        IEnumerable<SoccerChamp> ParseHead(CQ heads)
        {
            return heads.Map(domHead =>
                                 {
                                     var head = domHead.Cq();

                                     var headTitle = ParseBlockTitle(head);
                                     var eventNames = ParseEventsNamesForEvent(head);
                                     var lines = ParseLines(head, eventNames);

                                     return new SoccerChamp(headTitle, lines);
                                 });
        }

        static string ParseBlockTitle(CQ head)
        {
            return head.Find("div.block-events-head").Children().Remove().End().Text().Trim();
        }

        static IEnumerable<EventLine> ParseLines(CQ dom, IEnumerable<string> eventNames)
        {
            var events = dom.Select("table.foot-market tr.event-header");

            foreach (var @event in events)
            {
                var commands = ParseCommandsNames(dom, @event);

                var eventDate = ParseEventDate(dom, @event);

                var eventsData = ParseEvents(dom, @event).ToList();


                yield return
                    new EventLine(string.Join("-", commands), eventDate, CreateBets(eventNames, eventsData).ToArray());
            }
        }

        static string[] ParseCommandsNames(CQ dom, IDomObject @event)
        {
            var commands =
                dom.Select(
                    "td.today-name > span.command > div.today-member-name, td.name > span.command div.member-name",
                    @event).Map(node => node.Cq().Text().Trim().Replace(Nbsp, ' ')).ToArray();
            Debug.Assert(commands.Count() == 2, "Teams names have not been recognized: " + @event.InnerHTML);
            return commands;
        }

        static IEnumerable<ParsedEventData> ParseEvents(CQ dom, IDomObject @event)
        {
            return dom.Select("td.js-price", @event)
                      .Map(node =>
                               {
                                   var domNode = node.Cq();
                                   var coefficient = double.Parse(domNode.Find("span.selection-link").Text().Trim());
                                   var specification = domNode.Children().Remove().End().Text().Trim();
                                   return new ParsedEventData {Coefficient = coefficient, Specification = specification};
                               });
        }

        static DateTimeOffset ParseEventDate(CQ dom, IDomObject @event)
        {
            var date = dom.Select("td.date", @event).Text().Trim();

            DateTimeOffset eventDate;
            DateTimeOffset.TryParse(date, out eventDate);

            return eventDate;
        }

        static IEnumerable<Event> CreateBets(IEnumerable<string> eventsNames, IEnumerable<ParsedEventData> eventsData)
        {
            Debug.Assert(eventsNames.Count() == eventsData.Count(),
                         "Bets count and events count should match each other, but was " + eventsData.Count() + " vs " +
                         eventsNames.Count());

            return eventsNames.Zip(eventsData, (name, data) => new Event(name, data.Coefficient, data.Specification));
        }

        IEnumerable<string> ParseEventsNamesForEvent(CQ dom)
        {
            var tablebets = dom.Select("table.foot-market tbody:first", dom);

            var betsKind1 =
                dom.Select("th.coupone > div.markets-hint", tablebets)
                   .Map(node => node.Cq().Text().Trim().Replace(Nbsp, ValidSpace));
            var betsKind2 = dom.Select("th.coupone > span", tablebets).Map(node => node.Cq().Text());

            return betsKind1.Union(betsKind2).ToList();
        }

        static CQ CreateDomParser(Stream response)
        {
            return CQ.Create(new StreamReader(response),
                             parsingOptions: HtmlParsingOptions.IgnoreComments | HtmlParsingOptions.AllowSelfClosingTags);
        }

        struct ParsedEventData
        {
            public double Coefficient { get; set; }
            public string Specification { get; set; }
        }
    }
}