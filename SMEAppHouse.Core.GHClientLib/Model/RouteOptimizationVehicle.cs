using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationVehicle
    {
        [DataMember(Name = "vehicle_id")]
        public string VehicleId { get; set; }
        [DataMember(Name = "start_address")]
        public RouteOptimizationAddress StartAddress { get; set; }
    }
}