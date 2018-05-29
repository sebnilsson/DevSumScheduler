using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsQuery;
using DevSumScheduler.Data;

namespace DevSumScheduler.WebApp.Data
{
    public class HttpSpeakerDataProvider : ISpeakerDataProvider
    {
        private const string SpeakerUrlFormat = "http://www.devsum.se/speaker/{0}";

        public async Task<string> GetData(string slug)
        {
            var speakerUrl = string.Format(SpeakerUrlFormat, slug);

            var http = new HttpClient();

            var speakerData = await http.GetStringAsync(speakerUrl);

            var dom = CQ.CreateDocument(speakerData);
            
            var header = dom[".body-wrapper header"];
            var substitute = dom["#gdlr-header-substitute"];
            var titleWrapper = dom[".gdlr-page-title-wrapper"];
            var footer = dom[".footer-wrapper"];
            var buttons = dom[".gdlr-button"];
            var scripts = dom["script"];

            header.Remove();
            substitute.Remove();
            titleWrapper.Remove();
            footer.Remove();
            buttons.Remove();
            scripts.Remove();

            var startContent = dom[".gdlr-item-start-content"];
            startContent.Css("padding-top", "25px");
            startContent.Css("padding-bottom", "25px");

            var domHtml = string.Join(Environment.NewLine, dom.Elements.Select(x => x.OuterHTML));
            return domHtml;
        }
    }
}