﻿using Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate.Entities;

namespace Innermost.MusicHub.Domain.AggregatesModels.SingerAggregate
{
    public class Singer : TagableEntity<string>,IAggregateRoot
    {
        public int SingerId { get; private set; }
        /// <summary>
        /// Chinese name
        /// </summary>
        public string SingerName { get; private set; }
        public string SingerAlias { get; private set; }
        public string SingerNationality { get; private set; }
        public string SingerBirthplace { get; private set; }
        public string SingerOccupation { get; private set; }
        public string SingerBirthday { get; private set; }
        public string SingerRepresentativeWorks { get; private set; }
        public string SingerRegion { get; private set; }
        public string SingerCoverUrl { get; private set; }

        private readonly List<SingerAlbum> _singerAlbums;
        public IReadOnlyCollection<SingerAlbum> SingerAlbums => _singerAlbums;
        public Singer(string mid,int singerId, string singerName,string singerAlias,
            string singerNationality,string singerBirthplace,string singerOccupation,string singerBirthday,string singerRepresentativeWorks,
            string singerRegion,string singerCoverUrl,List<SingerAlbum> singerAlbums,List<TagSummary> tagSummaries):base(tagSummaries)
        {
            Id = mid;
            SingerId = singerId;
            SingerName = singerName;
            SingerAlias = singerAlias;
            SingerNationality = singerNationality;
            SingerBirthplace = singerBirthplace;
            SingerOccupation = singerOccupation;
            SingerBirthday = singerBirthday;
            SingerRepresentativeWorks = singerRepresentativeWorks;
            SingerRegion = singerRegion;
            SingerCoverUrl = singerCoverUrl;
            _singerAlbums=singerAlbums;
        }

        protected override IReferrer ToReferrer()
        {
            throw new NotImplementedException();
        }
    }
}
