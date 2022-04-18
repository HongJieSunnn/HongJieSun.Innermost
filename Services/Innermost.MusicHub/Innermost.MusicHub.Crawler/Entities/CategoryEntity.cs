namespace Innermost.MusicHub.Crawler.Entities
{
    internal class CategoryEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public CategoryEntity(int categoryId,string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            CategoryEntity other = (CategoryEntity)obj;
            return other.CategoryId.Equals(CategoryId);
        }
        public override int GetHashCode()
        {
            return CategoryId.GetHashCode();
        }
    }

    internal class MusicListEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Dissid { get; set; }
        public MusicListEntity(string dissid)
        {
            Dissid = dissid;
        }

        public override bool Equals(object? obj)
        {
            if(obj == null) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj.GetType() != GetType()) return false;
            MusicListEntity other = (MusicListEntity)obj;
            return Dissid.Equals(other.Dissid);
        }
        public override int GetHashCode()
        {
            return Dissid.GetHashCode();
        }
    }
}
