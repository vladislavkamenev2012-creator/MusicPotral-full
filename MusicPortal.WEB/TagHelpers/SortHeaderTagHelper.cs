using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MusicPortal.WEB.TagHelpers
{
    [HtmlTargetElement("sort-header", Attributes = "action, property, current-sort")]
    public class SortHeaderTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public SortHeaderTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = null!;

        public string Action { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public string CurrentSort { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "a";

            string nextSort = Property.ToLower() == CurrentSort.ToLower() ? $"{Property.ToLower()}_desc" : Property.ToLower();

            var routeValues = ViewContext.HttpContext.Request.Query.ToDictionary(q => q.Key, q => (object?)q.Value.ToString());
            routeValues["sortOrder"] = nextSort;

            output.Attributes.SetAttribute("href", urlHelper.Action(Action, routeValues));
            
            // Fixed: use output.Attributes.Add instead of AddCssClass
            output.Attributes.Add("class", "text-decoration-none text-dark fw-bold");

            if (CurrentSort == Property.ToLower())
            {
                output.PostContent.Append(" 🔼");
            }
            else if (CurrentSort == $"{Property.ToLower()}_desc")
            {
                output.PostContent.Append(" 🔽");
            }
        }
    }
}
