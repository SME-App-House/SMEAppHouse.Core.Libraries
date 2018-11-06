using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class Hit
    {
        [DataMember(Name = "osm_id")]
        public string OsmId { get; set; }
        [DataMember(Name = "extent")]
        public string[] Extent { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "city")]
        public string City { get; set; }
        [DataMember(Name = "postcode")]
        public string Postcode { get; set; }
        [DataMember(Name = "point")]
        public LngLatPoint Point { get; set; }
        [DataMember(Name = "osm_type")]
        public string OsmType { get; set; }
        [DataMember(Name = "osm_key")]
        public string OsmKey { get; set; }
        [DataMember(Name = "housenumber")]
        public string Housenumber { get; set; }
        [DataMember(Name = "street")]
        public string Street { get; set; }
        [DataMember(Name = "osm_value")]
        public string OsmValue { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
    }
}