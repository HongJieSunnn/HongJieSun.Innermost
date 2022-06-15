namespace Innermost.MusicHub.API.Queries.MusicRecordQueries.Models
{
    public class MusicRecordDTO
    {
        public string Mid { get; init; }
        public long MusicId { get; init; }
        public string MusicName { get; init; }
        public string? TranslatedMusicName { get; init; }
        public string? Introduction { get; init; }
        public string Genre { get; init; }
        public string Language { get; init; }
        public string AlbumCoverUrl { get; init; }
        public string MusicUrl { get; init; }
        public string? WikiUrl { get; init; }
        public string Lyric { get; init; }
        public List<MusicRecordSingerDTO> Singers { get; init; }
        public MusicRecordAlbumDTO Album { get; init; }
        public string PublishTime { get; init; }
        public List<TagSummaryDTO> TagSummaries { get; init; }
        public MusicRecordDTO(
            string mid, long musicId,
            string musicName, string? translatedMusicName,string? introduction,
            string genre, string language,
            string albumCoverUrl, string musicUrl, string? wikiUrl, string lyric,
            List<MusicRecordSingerDTO> singers, MusicRecordAlbumDTO album, string publishTime,List<TagSummaryDTO> tagSummaries)
        {
            Mid = mid;
            MusicName = musicName;
            MusicId = musicId;
            TranslatedMusicName = translatedMusicName;
            Introduction = introduction;
            Genre = genre;
            Language = language;
            AlbumCoverUrl = albumCoverUrl;
            MusicUrl = musicUrl;
            Lyric = lyric;
            Singers = (singers is not null || singers?.Count > 0) ? singers : throw new ArgumentException("Singers must have at least one singer and can not be null");
            Album = album ?? throw new ArgumentException("MusicRecord must contains in a album");
            PublishTime = publishTime;
            WikiUrl = wikiUrl;
            TagSummaries= tagSummaries;
        }
    }
    
    public class TagSummaryDTO
    {
        public string TagId { get; init; }
        public string TagName { get; init; }
        public TagSummaryDTO(string tagId,string tagName)
        {
            TagId=tagId;
            TagName=tagName;
        }
    }
}
