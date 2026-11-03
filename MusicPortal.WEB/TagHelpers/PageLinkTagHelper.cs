using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.WEB.Models;

namespace MusicPortal.WEB.TagHelpers
{
    [HtmlTargetElement("page-link", Attributes = "page-model, page-action")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public PageInfo PageModel { get; set; } = null!;
        public string PageAction { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("ul");
            result.AddCssClass("pagination");

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder item = new TagBuilder("li");
                item.AddCssClass("page-item");
                if (i == PageModel.PageNumber)
                {
                    item.AddCssClass("active");
                }

                TagBuilder link = new TagBuilder("a");
                link.AddCssClass("page-link");
                
                var routeValues = ViewContext.HttpContext.Request.Query.ToDictionary(q => q.Key, q => (object?)q.Value.ToString());
                routeValues["page"] = i;

                link.Attributes["href"] = urlHelper.Action(PageAction, routeValues);
                link.InnerHtml.Append(i.ToString());

                item.InnerHtml.AppendHtml(link);
                result.InnerHtml.AppendHtml(item);
            }

            output.TagName = "nav";
            output.Content.AppendHtml(result);
        }
    }
}
