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
    /// MatrixRequest
    /// </summary>
    [DataContract]
    public partial class MatrixRequest :  IEquatable<MatrixRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixRequest" /> class.
        /// </summary>
        /// <param name="Points">Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format longitude,latitude..</param>
        /// <param name="FromPoints">The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format longitude,latitude..</param>
        /// <param name="ToPoints">The destination points for the routes. Is a string with the format longitude,latitude..</param>
        /// <param name="OutArrays">Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API..</param>
        /// <param name="Vehicle">The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc, see here for the details..</param>
        public MatrixRequest(List<List<double?>> Points = default(List<List<double?>>), List<List<double?>> FromPoints = default(List<List<double?>>), List<List<double?>> ToPoints = default(List<List<double?>>), List<string> OutArrays = default(List<string>), string Vehicle = default(string))
        {
            this.Points = Points;
            this.FromPoints = FromPoints;
            this.ToPoints = ToPoints;
            this.OutArrays = OutArrays;
            this.Vehicle = Vehicle;
        }
        
        /// <summary>
        /// Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format longitude,latitude.
        /// </summary>
        /// <value>Specifiy multiple points for which the weight-, route-, time- or distance-matrix should be calculated. In this case the starts are identical to the destinations. If there are N points, then NxN entries will be calculated. The order of the point parameter is important. Specify at least three points. Cannot be used together with from_point or to_point. Is a string with the format longitude,latitude.</value>
        [DataMember(Name="points", EmitDefaultValue=false)]
        public List<List<double?>> Points { get; set; }

        /// <summary>
        /// The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format longitude,latitude.
        /// </summary>
        /// <value>The starting points for the routes. E.g. if you want to calculate the three routes A-&amp;gt;1, A-&amp;gt;2, A-&amp;gt;3 then you have one from_point parameter and three to_point parameters. Is a string with the format longitude,latitude.</value>
        [DataMember(Name="from_points", EmitDefaultValue=false)]
        public List<List<double?>> FromPoints { get; set; }

        /// <summary>
        /// The destination points for the routes. Is a string with the format longitude,latitude.
        /// </summary>
        /// <value>The destination points for the routes. Is a string with the format longitude,latitude.</value>
        [DataMember(Name="to_points", EmitDefaultValue=false)]
        public List<List<double?>> ToPoints { get; set; }

        /// <summary>
        /// Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API.
        /// </summary>
        /// <value>Specifies which arrays should be included in the response. Specify one or more of the following options &#39;weights&#39;, &#39;times&#39;, &#39;distances&#39;. To specify more than one array use e.g. out_array&#x3D;times&amp;amp;out_array&#x3D;distances. The units of the entries of distances are meters, of times are seconds and of weights is arbitrary and it can differ for different vehicles or versions of this API.</value>
        [DataMember(Name="out_arrays", EmitDefaultValue=false)]
        public List<string> OutArrays { get; set; }

        /// <summary>
        /// The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc, see here for the details.
        /// </summary>
        /// <value>The vehicle for which the route should be calculated. Other vehicles are foot, small_truck etc, see here for the details.</value>
        [DataMember(Name="vehicle", EmitDefaultValue=false)]
        public string Vehicle { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MatrixRequest {\n");
            sb.Append("  Points: ").Append(Points).Append("\n");
            sb.Append("  FromPoints: ").Append(FromPoints).Append("\n");
            sb.Append("  ToPoints: ").Append(ToPoints).Append("\n");
            sb.Append("  OutArrays: ").Append(OutArrays).Append("\n");
            sb.Append("  Vehicle: ").Append(Vehicle).Append("\n");
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
            return Equals(input as MatrixRequest);
        }

        /// <summary>
        /// Returns true if MatrixRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of MatrixRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(MatrixRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    Points == input.Points ||
                    Points != null &&
                    Points.SequenceEqual(input.Points)
                ) && 
                (
                    FromPoints == input.FromPoints ||
                    FromPoints != null &&
                    FromPoints.SequenceEqual(input.FromPoints)
                ) && 
                (
                    ToPoints == input.ToPoints ||
                    ToPoints != null &&
                    ToPoints.SequenceEqual(input.ToPoints)
                ) && 
                (
                    OutArrays == input.OutArrays ||
                    OutArrays != null &&
                    OutArrays.SequenceEqual(input.OutArrays)
                ) && 
                (
                    Vehicle == input.Vehicle ||
                    (Vehicle != null &&
                    Vehicle.Equals(input.Vehicle))
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
                if (Points != null)
                    hashCode = hashCode * 59 + Points.GetHashCode();
                if (FromPoints != null)
                    hashCode = hashCode * 59 + FromPoints.GetHashCode();
                if (ToPoints != null)
                    hashCode = hashCode * 59 + ToPoints.GetHashCode();
                if (OutArrays != null)
                    hashCode = hashCode * 59 + OutArrays.GetHashCode();
                if (Vehicle != null)
                    hashCode = hashCode * 59 + Vehicle.GetHashCode();
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
