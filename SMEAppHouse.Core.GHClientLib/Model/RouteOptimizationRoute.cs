using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationRoute
    {
        [DataMember(Name = "vehicle_id")]
        public string VehicleId { get; set; }
        [DataMember(Name = "activities")]
        public RouteOptimizationActivity[] Activities { get; set; }
    }
}