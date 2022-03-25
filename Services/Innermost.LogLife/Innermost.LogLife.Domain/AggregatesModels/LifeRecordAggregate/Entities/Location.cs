using DomainSeedWork.Abstractions;

namespace Innermost.LogLife.Domain.AggregatesModels.LifeRecordAggregate.Entities
{
    public class Location
        : Entity<string>
    {
        public string Name { get;private set; }
        public string Province { get; private set; }
        public string City { get; private set; }
        public string? District { get;private set; }
        public string Address { get;private set; }
        public BaiduPOI BaiduPOI { get;private set; }
        public Location()
        {

        }
        public Location(string uid,string name,string province,string city,string address,BaiduPOI baiduPOI,string? district=null)
        {
            Id = uid;
            Name = name;
            Province = province; 
            City = city;
            Address = address;
            District = district;
            BaiduPOI = baiduPOI;
            District= district;
        }
    }
}
