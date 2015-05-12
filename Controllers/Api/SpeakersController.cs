using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

using CsQuery;

namespace DevSumScheduler.Controllers.Api
{
    [Route("api/speakers/{action=index}", Name = "ApiSpeakers")]
    public class SpeakersController : ApiController
    {
        private static readonly Regex DevSumSpeakerUrlRegex = new Regex(@"^https?:\/\/(?:www.)?devsum.se/speaker/");

        [HttpGet]
        public async Task<IHttpActionResult> Index(string devSumSpeakerUrl)
        {
            if (string.IsNullOrWhiteSpace(devSumSpeakerUrl) || !DevSumSpeakerUrlRegex.IsMatch(devSumSpeakerUrl))
            {
                return this.BadRequest();
            }

            devSumSpeakerUrl = devSumSpeakerUrl.ToLowerInvariant();

            string speakerContent = await GetCachedSpeakerContent(devSumSpeakerUrl);
            return this.Ok(speakerContent);
        }

        private static async Task<string> GetCachedSpeakerContent(string devSumSpeakerUrl)
        {
            var cachedSpeakerContent = MemoryCache.Default[devSumSpeakerUrl] as string;

            if (cachedSpeakerContent == null)
            {
                var speakerContent = await GetSpeakerContent(devSumSpeakerUrl);

                MemoryCache.Default.Add(devSumSpeakerUrl, speakerContent, DateTime.Now.AddHours(1));

                cachedSpeakerContent = speakerContent;
            }

            return cachedSpeakerContent;
        }

        private static async Task<string> GetSpeakerContent(string devSumSpeakerUrl)
        {
            string html;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(devSumSpeakerUrl);
                html = await response.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            var csQuery = CQ.Create(html);

            csQuery["#gk-page-top"].Remove();
            csQuery["#gk-header-top"].Remove();
            csQuery["#gk-breadcrumbs"].Remove();
            csQuery["#gk-bottom-wrap"].Remove();

            csQuery["html"].Css("border", "0 !important");
            csQuery[".gk-page-wrap"].Css("padding", "0 !important");
            csQuery["section.content"].Css("padding", "0 !important");

            var bodyEl = csQuery["body"];
            var domDocument = bodyEl.Document;

            string documentHtml = string.Join(Environment.NewLine, domDocument.ChildElements.Select(x => x.OuterHTML));
            return documentHtml;
        }
    }
}