namespace Innermost.MusicHub.API.Queries.AlbumQueries.Models
{
    public class AlbumMusicRecordDTO
    {
        public string Mid { get; init; }
        public string MusicName { get; init; }
        public string? TranslatedMusicName { get; init; }
        public string Genre { get; init; }
        public string Language { get; init; }
        public string MusicUrl { get; init; }
        public List<AlbumSingerDTO> AlbumSingers { get; init; }
        public AlbumMusicRecordDTO(string mid, string musicName, string translatedMusicName, string genre, string language, string musicUrl, List<AlbumSingerDTO> singers)
        {
            Mid = mid;
            MusicName = musicName;
            TranslatedMusicName = translatedMusicName;
            Genre = genre;
            Language = language;
            MusicUrl = musicUrl;
            AlbumSingers = singers;
        }
    }
}
