namespace Innermost.Intelligence.API.Models
{
    public class TagSummary
    {
        public const string PositiveTagName = "心情:积极";
        public const string NeutralTagName = "心情:中性";
        public const string NegativeTagName = "心情:消极";
        public const string MixedTagName = "心情:Mixed";

        public static TagSummary Positive = new TagSummary("627a285c080d9c2bc6111928", PositiveTagName);
        public static TagSummary Neutral = new TagSummary("627a285c080d9c2bc6111929", NeutralTagName);
        public static TagSummary Negative = new TagSummary("627a285c080d9c2bc611192a", NegativeTagName);
        public static TagSummary Mixed = new TagSummary("627a285c080d9c2bc611192b", MixedTagName);

        public string TagId { get; set; }
        public string TagName { get; set; }
        public TagSummary(string tagId,string tagName)
        {
            TagId = tagId;
            TagName = tagName;
        }
    }
}
