namespace Innemost.LogLife.API.Queries.Models
{
    public class QueryModelMapper
    {
        public static LifeRecordDTO MapToLifeRecordDTO(dynamic record)
        {
            return new LifeRecordDTO(
                record.Id, record.Title, record.Text, record.IsShared,
                MapToLocationDTO(record),
                MapToMusicRecordDTO(record),
                record.ImagePaths?.Split(","),
                record.CreateTime,
                MapToTagSummaryDTOs(record)
            );
        }

        public static IEnumerable<LifeRecordDTO> MapToLifeRecordDTOs(IEnumerable<dynamic> records)
        {
            return records.Select<dynamic, LifeRecordDTO>(r => MapToLifeRecordDTO(r)).ToList();
        }

        private static LocationDTO? MapToLocationDTO(dynamic record)
        {
            if (record.LocationUId is null)
                return null;
            return new LocationDTO(
                record.LocationUId,
                record.LocationName,
                record.Province,
                record.City,
                record.District,
                record.Address,
                record.Longitude,
                record.Latitude
            );
        }

        private static MusicRecordDTO? MapToMusicRecordDTO(dynamic record)
        {
            if (record.MusicRecordMId is null)
                return null;
            return new MusicRecordDTO(
                record.MusicRecordMId,
                record.MusicName,
                record.Singer,
                record.Album
            );
        }

        private static List<TagSummaryDTO> MapToTagSummaryDTOs(dynamic record)
        {
            var tagSummaries = record.Tags.Split(",") as IEnumerable<string>;
            return tagSummaries is not null ?
                tagSummaries.Select(t =>
                {
                    var kv = t.Split('-');
                    return new TagSummaryDTO(kv[0], kv[1]);
                }).ToList()
                :
                throw new ArgumentException("One record must have at least one Tag,but read null now.(record:{@record})", record);
        }
    }
}
