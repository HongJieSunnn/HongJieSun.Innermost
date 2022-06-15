namespace Innermost.MusicHub.Crawler.Entities
{
    internal class MusicTagEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string TagName { get; set; }
        public List<string> MusicRecordMids { get; set; }
        public MusicTagEntity(string? id, string tagName, List<string> musicRecordMids)
        {
            Id = id;
            TagName = tagName;
            MusicRecordMids = musicRecordMids;
        }
    }
}
