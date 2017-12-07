using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FeatureToggle.Infrastructure
{
    public class FeatureTagHelper : TagHelper
    {
        private readonly IFeature _feature;
        [HtmlAttributeName("name")]
        public string Name { get; set; }
        public FeatureTagHelper(IFeature feature)
        {
            _feature = feature;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "DIV";
            if (!_feature.IsFeatureEnabled(Name))
            {
                output.SuppressOutput();
            }
        }
    }
}