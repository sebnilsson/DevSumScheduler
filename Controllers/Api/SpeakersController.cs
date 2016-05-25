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
        private const int CacheExpirationMinutes = 60;

        private static readonly NamedLock CacheLock = new NamedLock();

        private static readonly Regex DevSumSpeakerUrlRegex = new Regex(@"^https?:\/\/(?:www.)?devsum.se/speaker/");

        [HttpGet]
        public async Task<IHttpActionResult> Index(string devSumSpeakerUrl)
        {
            if (string.IsNullOrWhiteSpace(devSumSpeakerUrl) || !DevSumSpeakerUrlRegex.IsMatch(devSumSpeakerUrl))
            {
                return this.BadRequest();
            }

            devSumSpeakerUrl = devSumSpeakerUrl.ToLowerInvariant();

            string speakerContent = await GetSpeakerContent(devSumSpeakerUrl);
            return this.Ok(speakerContent);
        }

        private static async Task<string> GetSpeakerContent(string devSumSpeakerUrl)
        {
            string cacheKey = $"DevSumScheduler.Controllers.Api.SpeakersController?devSumSpeakerUrl={devSumSpeakerUrl}";

            var cachedSpeakerContent = MemoryCache.Default[cacheKey] as string;

            if (cachedSpeakerContent == null)
            {
                await CacheLock.RunWithLock(
                    cacheKey,
                    async () =>
                        {
                            cachedSpeakerContent = MemoryCache.Default[cacheKey] as string;

                            if (cachedSpeakerContent == null)
                            {
                                cachedSpeakerContent = await GetSpeakerContentInternal(devSumSpeakerUrl);

                                MemoryCache.Default.Add(
                                    cacheKey,
                                    cachedSpeakerContent,
                                    DateTime.Now.AddMinutes(CacheExpirationMinutes));
                            }
                        });
            }

            return cachedSpeakerContent;
        }

        private static async Task<string> GetSpeakerContentInternal(string devSumSpeakerUrl)
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

            csQuery["header"].Remove();
            csQuery["footer"].Remove();

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