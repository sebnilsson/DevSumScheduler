using System.Web;
using System.Web.Mvc;

using SquishIt.Framework;

namespace DevSumScheduler
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString CssBundleTag(this HtmlHelper htmlHelper, string bundleName)
        {
            var renderedCss = Bundle.Css().RenderCachedAssetTag(bundleName);
            return new HtmlString(renderedCss);
        }

        public static IHtmlString JsBundleTag(this HtmlHelper htmlHelper, string bundleName)
        {
            var renderedJs = Bundle.JavaScript().RenderCachedAssetTag(bundleName);
            return new HtmlString(renderedJs);
        }
    }
}