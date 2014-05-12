using HtmlAgilityPack;

namespace DevSumScheduler
{
    public static class HtmlNodeExtensions
    {
        public static HtmlNodeCollection SelectNodesEnsured(this HtmlNode htmlNode, string xpath)
        {
            return htmlNode.SelectNodes(xpath) ?? new HtmlNodeCollection(htmlNode);
        }
    }
}