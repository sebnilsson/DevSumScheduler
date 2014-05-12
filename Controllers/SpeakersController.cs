using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

using HtmlAgilityPack;

namespace DevSumScheduler.Controllers
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

            var cachedSpeakerContent = MemoryCache.Default[devSumSpeakerUrl] as string;

            if (cachedSpeakerContent == null)
            {
                string speakerContent = await GetSpeakerContent(devSumSpeakerUrl);

                MemoryCache.Default.Add(devSumSpeakerUrl, speakerContent, DateTime.Now.AddHours(1));

                cachedSpeakerContent = speakerContent;
            }

            return this.Ok(cachedSpeakerContent);
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

            var document = new HtmlDocument();
            document.LoadHtml(html);

            var bodyNode = document.DocumentNode.SelectSingleNode("//body");

            var bodyChildren = bodyNode.SelectNodes("//body/div");

            var contentNode = bodyNode.SelectSingleNode("//div[@class='gk-page']//article");
            var contentNodeParent = GetTopLevelParent(contentNode, bodyNode);

            var bodyChildrenRemove = bodyChildren.Where(x => x != contentNodeParent).ToList();
            bodyChildrenRemove.ForEach(x => x.Remove());

            var breadCrumbsNode = bodyNode.SelectSingleNode("//section[@id='gk-breadcrumbs']");
            if (breadCrumbsNode != null)
            {
                breadCrumbsNode.Remove();
            }

            //bodyNode.ReplaceChild(contentNode, contentNodeParent);
            return document.DocumentNode.OuterHtml;
        }

        private static HtmlNode GetTopLevelParent(HtmlNode htmlNode, HtmlNode bodyNode)
        {
            var parent = htmlNode.ParentNode;
            while (parent != null && parent.ParentNode != null && parent.ParentNode != bodyNode)
            {
                parent = parent.ParentNode;
            }

            return parent;
        }
    }
}