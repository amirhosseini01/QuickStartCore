using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Server.TagHelpers;

public class ModalTagHelper : TagHelper
{
    [HtmlAttributeName("id")] public string ModalId { get; set; } = "modal";
    // sm, lg, xl, fullscreen
    [HtmlAttributeName("modalSize")] public string ModalSize { get; set; } = "xl";
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        output.Attributes.SetAttribute("id", ModalId);
        output.Attributes.SetAttribute("class", "modal fade");
        output.Attributes.SetAttribute("tabindex", "-1");

        output.PreContent.SetHtmlContent(
            $"<div class=\"modal-dialog modal-dialog-scrollable modal-{ModalSize}\"><div class=\"modal-content\">");

        var childContent = output.Content.IsModified
            ? output.Content.GetContent()
            : (await output.GetChildContentAsync()).GetContent();
        output.Content.SetHtmlContent(childContent);

        output.PostContent.SetHtmlContent("</div></div>");
    }
}

[HtmlTargetElement("modalHeader")]
public class ModalHeaderTagHelper : TagHelper
{
    [HtmlAttributeName("id")] public string ModalHeaderId { get; set; } = "modalHeader";
    [HtmlAttributeName("title")] public string ModalHeaderTitle { get; set; }


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "modal-header");

        //title
        output.PreContent.SetHtmlContent($"<h5 id=\"{ModalHeaderId}\" class=\"modal-title\">{ModalHeaderTitle}</h5>");
    }
}

[HtmlTargetElement("modalBody")]
public class ModalBodyTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "modal-body");
    }
}

[HtmlTargetElement("modalFooter")]
public class ModalFooterTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "modal-footer");
    }
}