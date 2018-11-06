using System.Runtime.Serialization;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract]
    public class RouteOptimizationResponse
    {
        [DataMember(Name = "job_id")]
        public string JobId { get; set; }
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "waiting_time_in_queue")]
        public string WaitingTimeInQueue { get; set; }
        [DataMember(Name = "processing_time")]
        public string ProcessingTime { get; set; }
        [DataMember(Name = "solution")]
        public RouteOptimizationSolution Solution { get; set; }
    }
}