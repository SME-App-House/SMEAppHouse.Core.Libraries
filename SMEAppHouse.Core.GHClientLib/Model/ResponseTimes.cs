using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BNS.POW.GHClientLibV2.Model
{

    public interface IReponseDetail
    {
    }

    /// <summary>
    /// ResponseCoordinates
    /// </summary>
    [DataContract]
    public partial class ResponseTimes : IEquatable<ResponseTimes>, IValidatableObject, IReponseDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTimes" /> class.
        /// </summary>
        /// <param name="times">Coordinates.</param>
        public ResponseTimes(ResponseTimesArray times = default(ResponseTimesArray))
        {
            this.Times = times;
        }


        /// <summary>
        /// Gets or Sets Coordinates
        /// </summary>
        [DataMember(Name = "times", EmitDefaultValue = false)]
        public ResponseTimesArray Times { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ResponseTimes {\n");
            sb.Append("  Times: ").Append(Times).Append("\n");
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
            return Equals(input as ResponseTimes);
        }

        /// <summary>
        /// Returns true if ResponseTimes instances are equal
        /// </summary>
        /// <param name="input">Instance of ResponseTimes to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ResponseTimes input)
        {
            if (input == null)
                return false;

            return
            (
                Equals(Times, input.Times) ||
                (Times != null &&
                 Times.Equals(input.Times))
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
                if (Times != null)
                    hashCode = hashCode * 59 + Times.GetHashCode();
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