using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.TagHelpers
{
    [HtmlTargetElement("img", Attributes = "gravatar-email")]
    public class GravatarTagHelper : TagHelper
    {
        [HtmlAttributeName("gravatar-email")]
        public string Email { get; set; }
        [HtmlAttributeName("gravatar-mode")]
        public Mode Mode { get; set; } = Mode.Mm;
        [HtmlAttributeName("gravatar-rating")]
        public Rating Rating { get; set; } = Rating.g;
        [HtmlAttributeName("gravatar-size")]
        public int Size { get; set; } = 50;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(Email));
                var hash = BitConverter.ToString(result).Replace("-", "").ToLower();
                var url = $"http://gravatar.com/avatar/{hash}";
                var queryBuilder = new QueryBuilder();
                queryBuilder.Add("s", Size.ToString());
                queryBuilder.Add("d", GetModeValue(Mode));
                queryBuilder.Add("r", Rating.ToString());
                url = url + Uri.EscapeUriString(queryBuilder.ToQueryString().ToString());
                output.Attributes.SetAttribute("src", url);
            }
        }

        private static string GetModeValue(Mode mode)
        {
            if (mode == Mode.NotFound)
            {
                return "404";
            }

            return mode.ToString().ToLower();
        }
    }

    public enum Rating
    {
        g,
        pg,
        r,
        x
    }

    public enum Mode
    {
        [Display(Name = "404")]
        NotFound,
        [Display(Name = "Mm")]
        Mm,
        [Display(Name = "Identicon")]
        Identicon,
        [Display(Name = "Monsterid")]
        Monsterid,
        [Display(Name = "Wavatar")]
        Wavatar,
        [Display(Name = "Retro")]
        Retro,
        [Display(Name = "Blank")]
        Blank
    }
}