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
    /// IsochroneResponsePolygonGeometry
    /// </summary>
    [DataContract]
    public partial class IsochroneResponsePolygonGeometry :  IEquatable<IsochroneResponsePolygonGeometry>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsochroneResponsePolygonGeometry" /> class.
        /// </summary>
        /// <param name="Type">Type.</param>
        /// <param name="Coordinates">Coordinates.</param>
        public IsochroneResponsePolygonGeometry(string Type = default(string), List<ResponseCoordinatesArray> Coordinates = default(List<ResponseCoordinatesArray>))
        {
            this.Type = Type;
            this.Coordinates = Coordinates;
        }
        
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Coordinates
        /// </summary>
        [DataMember(Name="coordinates", EmitDefaultValue=false)]
        public List<ResponseCoordinatesArray> Coordinates { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class IsochroneResponsePolygonGeometry {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Coordinates: ").Append(Coordinates).Append("\n");
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
            return Equals(input as IsochroneResponsePolygonGeometry);
        }

        /// <summary>
        /// Returns true if IsochroneResponsePolygonGeometry instances are equal
        /// </summary>
        /// <param name="input">Instance of IsochroneResponsePolygonGeometry to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(IsochroneResponsePolygonGeometry input)
        {
            if (input == null)
                return false;

            return 
                (
                    Type == input.Type ||
                    (Type != null &&
                    Type.Equals(input.Type))
                ) && 
                (
                    Coordinates == input.Coordinates ||
                    Coordinates != null &&
                    Coordinates.SequenceEqual(input.Coordinates)
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
                if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                if (Coordinates != null)
                    hashCode = hashCode * 59 + Coordinates.GetHashCode();
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
