using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationSolution
    {
        [DataMember(Name = "costs")]
        public string Costs { get; set; }
        [DataMember(Name = "distance")]
        public string Distance { get; set; }
        [DataMember(Name = "time")]
        public string Time { get; set; }
        [DataMember(Name = "no_unassigned")]
        public string CntUnassigned { get; set; }
        [DataMember(Name = "routes")]
        public RouteOptimizationRoute[] Routes { get; set; }
    }
}