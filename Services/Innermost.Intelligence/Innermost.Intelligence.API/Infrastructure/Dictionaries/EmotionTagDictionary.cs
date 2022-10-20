namespace Innermost.Intelligence.API.Infrastructure.Dictionaries
{
    public class EmotionTagDictionary
    {
        public static Dictionary<TextSentiment, TagSummary> EmotionTags = new Dictionary<TextSentiment, TagSummary>()
        {
            { TextSentiment.Positive,TagSummary.Positive },
            { TextSentiment.Neutral,TagSummary.Neutral },
            { TextSentiment.Negative,TagSummary.Negative },
            { TextSentiment.Mixed,TagSummary.Mixed },
        };
    }
}
