/* 
 * GraphHopper Directions API
 *
 * You use the GraphHopper Directions API to add route planning, navigation and route optimization to your software. E.g. the Routing API has turn instructions and elevation data and the Route Optimization API solves your logistic problems and supports various constraints like time window and capacity restrictions. Also it is possible to get all distances between all locations with our fast Matrix API.
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    /// <summary>
    /// TimeWindow
    /// </summary>
    [DataContract]
    public partial class TimeWindow :  IEquatable<TimeWindow>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeWindow" /> class.
        /// </summary>
        /// <param name="Earliest">earliest start time of corresponding activity.</param>
        /// <param name="Latest">latest start time of corresponding activity.</param>
        public TimeWindow(long? Earliest = default(long?), long? Latest = default(long?))
        {
            this.Earliest = Earliest;
            this.Latest = Latest;
        }
        
        /// <summary>
        /// earliest start time of corresponding activity
        /// </summary>
        /// <value>earliest start time of corresponding activity</value>
        [DataMember(Name="earliest", EmitDefaultValue=false)]
        public long? Earliest { get; set; }

        /// <summary>
        /// latest start time of corresponding activity
        /// </summary>
        /// <value>latest start time of corresponding activity</value>
        [DataMember(Name="latest", EmitDefaultValue=false)]
        public long? Latest { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class TimeWindow {\n");
            sb.Append("  Earliest: ").Append(Earliest).Append("\n");
            sb.Append("  Latest: ").Append(Latest).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as TimeWindow);
        }

        /// <summary>
        /// Returns true if TimeWindow instances are equal
        /// </summary>
        /// <param name="input">Instance of TimeWindow to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TimeWindow input)
        {
            if (input == null)
                return false;

            return 
                (
                    Earliest == input.Earliest ||
                    (Earliest != null &&
                    Earliest.Equals(input.Earliest))
                ) && 
                (
                    Latest == input.Latest ||
                    (Latest != null &&
                    Latest.Equals(input.Latest))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (Earliest != null)
                    hashCode = hashCode * 59 + Earliest.GetHashCode();
                if (Latest != null)
                    hashCode = hashCode * 59 + Latest.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
