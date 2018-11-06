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
    /// ResponseInstruction
    /// </summary>
    [DataContract]
    public partial class ResponseInstruction :  IEquatable<ResponseInstruction>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseInstruction" /> class.
        /// </summary>
        /// <param name="Text">A description what the user has to do in order to follow the route. The language depends on the locale parameter..</param>
        /// <param name="StreetName">The name of the street to turn onto in order to follow the route..</param>
        /// <param name="Distance">The distance for this instruction, in meter.</param>
        /// <param name="Time">The duration for this instruction, in ms.</param>
        /// <param name="Interval">An array containing the first and the last index (relative to paths[0].points) of the points for this instruction. This is useful to know for which part of the route the instructions are valid..</param>
        /// <param name="Sign">A number which specifies the sign to show e.g. for right turn etc &lt;br&gt;TURN_SHARP_LEFT &#x3D; -3&lt;br&gt;TURN_LEFT &#x3D; -2&lt;br&gt;TURN_SLIGHT_LEFT &#x3D; -1&lt;br&gt;CONTINUE_ON_STREET &#x3D; 0&lt;br&gt;TURN_SLIGHT_RIGHT &#x3D; 1&lt;br&gt;TURN_RIGHT &#x3D; 2&lt;br&gt;TURN_SHARP_RIGHT &#x3D; 3&lt;br&gt;FINISH &#x3D; 4&lt;br&gt;VIA_REACHED &#x3D; 5&lt;br&gt;USE_ROUNDABOUT &#x3D; 6.</param>
        /// <param name="AnnotationText">optional - A text describing the instruction in more detail, e.g. like surface of the way, warnings or involved costs..</param>
        /// <param name="AnnotationImportance">optional - 0 stands for INFO, 1 for warning, 2 for costs, 3 for costs and warning.</param>
        /// <param name="ExitNumber">optional - Only available for USE_ROUNDABOUT instructions. The count of exits at which the route leaves the roundabout..</param>
        /// <param name="TurnAngle">optional - Only available for USE_ROUNDABOUT instructions. The radian of the route within the roundabout - 0&amp;lt;r&amp;lt;2*PI for clockwise and -2PI&amp;lt;r&amp;lt;0 for counterclockwise transit. Null if the direction of rotation is undefined..</param>
        public ResponseInstruction(string Text = default(string), string StreetName = default(string), double? Distance = default(double?), int? Time = default(int?), List<int?> Interval = default(List<int?>), int? Sign = default(int?), string AnnotationText = default(string), int? AnnotationImportance = default(int?), int? ExitNumber = default(int?), double? TurnAngle = default(double?))
        {
            this.Text = Text;
            this.StreetName = StreetName;
            this.Distance = Distance;
            this.Time = Time;
            this.Interval = Interval;
            this.Sign = Sign;
            this.AnnotationText = AnnotationText;
            this.AnnotationImportance = AnnotationImportance;
            this.ExitNumber = ExitNumber;
            this.TurnAngle = TurnAngle;
        }
        
        /// <summary>
        /// A description what the user has to do in order to follow the route. The language depends on the locale parameter.
        /// </summary>
        /// <value>A description what the user has to do in order to follow the route. The language depends on the locale parameter.</value>
        [DataMember(Name="text", EmitDefaultValue=false)]
        public string Text { get; set; }

        /// <summary>
        /// The name of the street to turn onto in order to follow the route.
        /// </summary>
        /// <value>The name of the street to turn onto in order to follow the route.</value>
        [DataMember(Name="street_name", EmitDefaultValue=false)]
        public string StreetName { get; set; }

        /// <summary>
        /// The distance for this instruction, in meter
        /// </summary>
        /// <value>The distance for this instruction, in meter</value>
        [DataMember(Name="distance", EmitDefaultValue=false)]
        public double? Distance { get; set; }

        /// <summary>
        /// The duration for this instruction, in ms
        /// </summary>
        /// <value>The duration for this instruction, in ms</value>
        [DataMember(Name="time", EmitDefaultValue=false)]
        public int? Time { get; set; }

        /// <summary>
        /// An array containing the first and the last index (relative to paths[0].points) of the points for this instruction. This is useful to know for which part of the route the instructions are valid.
        /// </summary>
        /// <value>An array containing the first and the last index (relative to paths[0].points) of the points for this instruction. This is useful to know for which part of the route the instructions are valid.</value>
        [DataMember(Name="interval", EmitDefaultValue=false)]
        public List<int?> Interval { get; set; }

        /// <summary>
        /// A number which specifies the sign to show e.g. for right turn etc &lt;br&gt;TURN_SHARP_LEFT &#x3D; -3&lt;br&gt;TURN_LEFT &#x3D; -2&lt;br&gt;TURN_SLIGHT_LEFT &#x3D; -1&lt;br&gt;CONTINUE_ON_STREET &#x3D; 0&lt;br&gt;TURN_SLIGHT_RIGHT &#x3D; 1&lt;br&gt;TURN_RIGHT &#x3D; 2&lt;br&gt;TURN_SHARP_RIGHT &#x3D; 3&lt;br&gt;FINISH &#x3D; 4&lt;br&gt;VIA_REACHED &#x3D; 5&lt;br&gt;USE_ROUNDABOUT &#x3D; 6
        /// </summary>
        /// <value>A number which specifies the sign to show e.g. for right turn etc &lt;br&gt;TURN_SHARP_LEFT &#x3D; -3&lt;br&gt;TURN_LEFT &#x3D; -2&lt;br&gt;TURN_SLIGHT_LEFT &#x3D; -1&lt;br&gt;CONTINUE_ON_STREET &#x3D; 0&lt;br&gt;TURN_SLIGHT_RIGHT &#x3D; 1&lt;br&gt;TURN_RIGHT &#x3D; 2&lt;br&gt;TURN_SHARP_RIGHT &#x3D; 3&lt;br&gt;FINISH &#x3D; 4&lt;br&gt;VIA_REACHED &#x3D; 5&lt;br&gt;USE_ROUNDABOUT &#x3D; 6</value>
        [DataMember(Name="sign", EmitDefaultValue=false)]
        public int? Sign { get; set; }

        /// <summary>
        /// optional - A text describing the instruction in more detail, e.g. like surface of the way, warnings or involved costs.
        /// </summary>
        /// <value>optional - A text describing the instruction in more detail, e.g. like surface of the way, warnings or involved costs.</value>
        [DataMember(Name="annotation_text", EmitDefaultValue=false)]
        public string AnnotationText { get; set; }

        /// <summary>
        /// optional - 0 stands for INFO, 1 for warning, 2 for costs, 3 for costs and warning
        /// </summary>
        /// <value>optional - 0 stands for INFO, 1 for warning, 2 for costs, 3 for costs and warning</value>
        [DataMember(Name="annotation_importance", EmitDefaultValue=false)]
        public int? AnnotationImportance { get; set; }

        /// <summary>
        /// optional - Only available for USE_ROUNDABOUT instructions. The count of exits at which the route leaves the roundabout.
        /// </summary>
        /// <value>optional - Only available for USE_ROUNDABOUT instructions. The count of exits at which the route leaves the roundabout.</value>
        [DataMember(Name="exit_number", EmitDefaultValue=false)]
        public int? ExitNumber { get; set; }

        /// <summary>
        /// optional - Only available for USE_ROUNDABOUT instructions. The radian of the route within the roundabout - 0&amp;lt;r&amp;lt;2*PI for clockwise and -2PI&amp;lt;r&amp;lt;0 for counterclockwise transit. Null if the direction of rotation is undefined.
        /// </summary>
        /// <value>optional - Only available for USE_ROUNDABOUT instructions. The radian of the route within the roundabout - 0&amp;lt;r&amp;lt;2*PI for clockwise and -2PI&amp;lt;r&amp;lt;0 for counterclockwise transit. Null if the direction of rotation is undefined.</value>
        [DataMember(Name="turn_angle", EmitDefaultValue=false)]
        public double? TurnAngle { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ResponseInstruction {\n");
            sb.Append("  Text: ").Append(Text).Append("\n");
            sb.Append("  StreetName: ").Append(StreetName).Append("\n");
            sb.Append("  Distance: ").Append(Distance).Append("\n");
            sb.Append("  Time: ").Append(Time).Append("\n");
            sb.Append("  Interval: ").Append(Interval).Append("\n");
            sb.Append("  Sign: ").Append(Sign).Append("\n");
            sb.Append("  AnnotationText: ").Append(AnnotationText).Append("\n");
            sb.Append("  AnnotationImportance: ").Append(AnnotationImportance).Append("\n");
            sb.Append("  ExitNumber: ").Append(ExitNumber).Append("\n");
            sb.Append("  TurnAngle: ").Append(TurnAngle).Append("\n");
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
            return Equals(input as ResponseInstruction);
        }

        /// <summary>
        /// Returns true if ResponseInstruction instances are equal
        /// </summary>
        /// <param name="input">Instance of ResponseInstruction to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResponseInstruction input)
        {
            if (input == null)
                return false;

            return 
                (
                    Text == input.Text ||
                    (Text != null &&
                    Text.Equals(input.Text))
                ) && 
                (
                    StreetName == input.StreetName ||
                    (StreetName != null &&
                    StreetName.Equals(input.StreetName))
                ) && 
                (
                    Distance == input.Distance ||
                    (Distance != null &&
                    Distance.Equals(input.Distance))
                ) && 
                (
                    Time == input.Time ||
                    (Time != null &&
                    Time.Equals(input.Time))
                ) && 
                (
                    Interval == input.Interval ||
                    Interval != null &&
                    Interval.SequenceEqual(input.Interval)
                ) && 
                (
                    Sign == input.Sign ||
                    (Sign != null &&
                    Sign.Equals(input.Sign))
                ) && 
                (
                    AnnotationText == input.AnnotationText ||
                    (AnnotationText != null &&
                    AnnotationText.Equals(input.AnnotationText))
                ) && 
                (
                    AnnotationImportance == input.AnnotationImportance ||
                    (AnnotationImportance != null &&
                    AnnotationImportance.Equals(input.AnnotationImportance))
                ) && 
                (
                    ExitNumber == input.ExitNumber ||
                    (ExitNumber != null &&
                    ExitNumber.Equals(input.ExitNumber))
                ) && 
                (
                    TurnAngle == input.TurnAngle ||
                    (TurnAngle != null &&
                    TurnAngle.Equals(input.TurnAngle))
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
                var hashCode = 41;
                if (Text != null)
                    hashCode = hashCode * 59 + Text.GetHashCode();
                if (StreetName != null)
                    hashCode = hashCode * 59 + StreetName.GetHashCode();
                if (Distance != null)
                    hashCode = hashCode * 59 + Distance.GetHashCode();
                if (Time != null)
                    hashCode = hashCode * 59 + Time.GetHashCode();
                if (Interval != null)
                    hashCode = hashCode * 59 + Interval.GetHashCode();
                if (Sign != null)
                    hashCode = hashCode * 59 + Sign.GetHashCode();
                if (AnnotationText != null)
                    hashCode = hashCode * 59 + AnnotationText.GetHashCode();
                if (AnnotationImportance != null)
                    hashCode = hashCode * 59 + AnnotationImportance.GetHashCode();
                if (ExitNumber != null)
                    hashCode = hashCode * 59 + ExitNumber.GetHashCode();
                if (TurnAngle != null)
                    hashCode = hashCode * 59 + TurnAngle.GetHashCode();
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
