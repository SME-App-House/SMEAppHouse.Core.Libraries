using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SMEAppHouse.Core.GHClientLib.Model.Interfaces;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    [DataContract(Name = "configuration")]
    public class RouteOptimizationConfiguration : IConfiguration
    {
        public Guid Uid => Guid.NewGuid();

        [DataMember(Name = "routing", EmitDefaultValue = false)]
        public IConfiguration ConfigurationMember { get; set; }

        #region constructors

        public RouteOptimizationConfiguration() : this(true)
        {
        }

        public RouteOptimizationConfiguration(bool calcPoints)
        {
            ConfigurationMember = new RoutingCfgMember(calcPoints: calcPoints);
        }

        #endregion

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        [DataContract(Name = "routing")]
        public class RoutingCfgMember : IConfiguration
        {
            public Guid Uid => Guid.NewGuid();

            public IConfiguration ConfigurationMember { get; set; }

            [DataMember(Name = "calc_points", EmitDefaultValue = false)]
            public bool CalculatePoints { get; set; }

            public RoutingCfgMember(bool calcPoints = true)
            {
                CalculatePoints = calcPoints;
            }

            /// <summary>
            /// Returns the JSON string presentation of the object
            /// </summary>
            /// <returns>JSON string presentation of the object</returns>
            public string ToJson()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }

        }



    }
}