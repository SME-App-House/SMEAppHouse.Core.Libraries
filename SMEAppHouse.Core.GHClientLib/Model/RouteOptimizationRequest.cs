using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationRequest
    {
        [DataMember(Name = "vehicles")]
        public RouteOptimizationVehicle[] Vehicles { get; set; }
        [DataMember(Name = "services")]
        public RouteOptimizationServiceEndPoint[] Services { get; set; }
    }
}