using HeyRed.MarkdownSharp;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TagHelperDemo.TagHelpers
{
    [HtmlTargetElement("markdown")]
    public class MarkdownTagHelper : TagHelper
    {
        [HtmlAttributeName("text")]
        public string Text { get; set; }

        [HtmlAttributeName("source")]
        public ModelExpression Source { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var markdownTransformer = new Markdown();
            if (Source != null)
            {
                Text = Source.Model.ToString();
            }

            var result = markdownTransformer.Transform(Text);
            output.TagName = "div";
            output.Content.SetHtmlContent(result);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}