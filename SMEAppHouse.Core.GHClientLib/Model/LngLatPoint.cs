using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class LngLatPoint
    {
        public LngLatPoint(double? lng = default(double), double? lat = default(double))
        {
            if (lng != null) this.Lng = (double)lng;
            if (lat != null) this.Lat = (double)lat;
        }

        [DataMember(Name = "lng")]
        public double Lng { get; set; }

        [DataMember(Name = "lat")]
        public double Lat { get; set; }
    }
}