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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace SMEAppHouse.Core.GHClientLib.Model
{
    /// <summary>
    /// SolutionUnassigned
    /// </summary>
    [DataContract]
    public partial class SolutionUnassigned :  IEquatable<SolutionUnassigned>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionUnassigned" /> class.
        /// </summary>
        /// <param name="Services">An array of ids of unassigned services.</param>
        /// <param name="Shipments">An array of ids of unassigned shipments.</param>
        public SolutionUnassigned(List<string> Services = default(List<string>), List<string> Shipments = default(List<string>))
        {
            this.Services = Services;
            this.Shipments = Shipments;
        }
        
        /// <summary>
        /// An array of ids of unassigned services
        /// </summary>
        /// <value>An array of ids of unassigned services</value>
        [DataMember(Name="services", EmitDefaultValue=false)]
        public List<string> Services { get; set; }

        /// <summary>
        /// An array of ids of unassigned shipments
        /// </summary>
        /// <value>An array of ids of unassigned shipments</value>
        [DataMember(Name="shipments", EmitDefaultValue=false)]
        public List<string> Shipments { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SolutionUnassigned {\n");
            sb.Append("  Services: ").Append(Services).Append("\n");
            sb.Append("  Shipments: ").Append(Shipments).Append("\n");
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
            return Equals(input as SolutionUnassigned);
        }

        /// <summary>
        /// Returns true if SolutionUnassigned instances are equal
        /// </summary>
        /// <param name="input">Instance of SolutionUnassigned to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(SolutionUnassigned input)
        {
            if (input == null)
                return false;

            return 
                (
                    Services == input.Services ||
                    Services != null &&
                    Services.SequenceEqual(input.Services)
                ) && 
                (
                    Shipments == input.Shipments ||
                    Shipments != null &&
                    Shipments.SequenceEqual(input.Shipments)
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
                if (Services != null)
                    hashCode = hashCode * 59 + Services.GetHashCode();
                if (Shipments != null)
                    hashCode = hashCode * 59 + Shipments.GetHashCode();
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
