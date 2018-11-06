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
    /// Shipment
    /// </summary>
    [DataContract]
    public partial class Shipment :  IEquatable<Shipment>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Shipment" /> class.
        /// </summary>
        /// <param name="Id">Unique identifier of service.</param>
        /// <param name="Name">name of shipment.</param>
        /// <param name="Priority">priority of service, i.e. 1 &#x3D; high, 2 &#x3D; normal, 3 &#x3D; low. default is 2..</param>
        /// <param name="Pickup">Pickup.</param>
        /// <param name="Delivery">Delivery.</param>
        /// <param name="Size">array of capacity dimensions.</param>
        /// <param name="RequiredSkills">array of required skills.</param>
        /// <param name="AllowedVehicles">array of allowed vehicle ids.</param>
        /// <param name="DisallowedVehicles">array of disallowed vehicle ids.</param>
        /// <param name="MaxTimeInVehicle">max time shipment can stay in vehicle.</param>
        public Shipment(string Id = default(string), string Name = default(string), int? Priority = default(int?), Stop Pickup = default(Stop), Stop Delivery = default(Stop), List<int?> Size = default(List<int?>), List<string> RequiredSkills = default(List<string>), List<string> AllowedVehicles = default(List<string>), List<string> DisallowedVehicles = default(List<string>), long? MaxTimeInVehicle = default(long?))
        {
            this.Id = Id;
            this.Name = Name;
            this.Priority = Priority;
            this.Pickup = Pickup;
            this.Delivery = Delivery;
            this.Size = Size;
            this.RequiredSkills = RequiredSkills;
            this.AllowedVehicles = AllowedVehicles;
            this.DisallowedVehicles = DisallowedVehicles;
            this.MaxTimeInVehicle = MaxTimeInVehicle;
        }
        
        /// <summary>
        /// Unique identifier of service
        /// </summary>
        /// <value>Unique identifier of service</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// name of shipment
        /// </summary>
        /// <value>name of shipment</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// priority of service, i.e. 1 &#x3D; high, 2 &#x3D; normal, 3 &#x3D; low. default is 2.
        /// </summary>
        /// <value>priority of service, i.e. 1 &#x3D; high, 2 &#x3D; normal, 3 &#x3D; low. default is 2.</value>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets Pickup
        /// </summary>
        [DataMember(Name="pickup", EmitDefaultValue=false)]
        public Stop Pickup { get; set; }

        /// <summary>
        /// Gets or Sets Delivery
        /// </summary>
        [DataMember(Name="delivery", EmitDefaultValue=false)]
        public Stop Delivery { get; set; }

        /// <summary>
        /// array of capacity dimensions
        /// </summary>
        /// <value>array of capacity dimensions</value>
        [DataMember(Name="size", EmitDefaultValue=false)]
        public List<int?> Size { get; set; }

        /// <summary>
        /// array of required skills
        /// </summary>
        /// <value>array of required skills</value>
        [DataMember(Name="required_skills", EmitDefaultValue=false)]
        public List<string> RequiredSkills { get; set; }

        /// <summary>
        /// array of allowed vehicle ids
        /// </summary>
        /// <value>array of allowed vehicle ids</value>
        [DataMember(Name="allowed_vehicles", EmitDefaultValue=false)]
        public List<string> AllowedVehicles { get; set; }

        /// <summary>
        /// array of disallowed vehicle ids
        /// </summary>
        /// <value>array of disallowed vehicle ids</value>
        [DataMember(Name="disallowed_vehicles", EmitDefaultValue=false)]
        public List<string> DisallowedVehicles { get; set; }

        /// <summary>
        /// max time shipment can stay in vehicle
        /// </summary>
        /// <value>max time shipment can stay in vehicle</value>
        [DataMember(Name="max_time_in_vehicle", EmitDefaultValue=false)]
        public long? MaxTimeInVehicle { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Shipment {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  Pickup: ").Append(Pickup).Append("\n");
            sb.Append("  Delivery: ").Append(Delivery).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  RequiredSkills: ").Append(RequiredSkills).Append("\n");
            sb.Append("  AllowedVehicles: ").Append(AllowedVehicles).Append("\n");
            sb.Append("  DisallowedVehicles: ").Append(DisallowedVehicles).Append("\n");
            sb.Append("  MaxTimeInVehicle: ").Append(MaxTimeInVehicle).Append("\n");
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
            return Equals(input as Shipment);
        }

        /// <summary>
        /// Returns true if Shipment instances are equal
        /// </summary>
        /// <param name="input">Instance of Shipment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Shipment input)
        {
            if (input == null)
                return false;

            return 
                (
                    Id == input.Id ||
                    (Id != null &&
                    Id.Equals(input.Id))
                ) && 
                (
                    Name == input.Name ||
                    (Name != null &&
                    Name.Equals(input.Name))
                ) && 
                (
                    Priority == input.Priority ||
                    (Priority != null &&
                    Priority.Equals(input.Priority))
                ) && 
                (
                    Pickup == input.Pickup ||
                    (Pickup != null &&
                    Pickup.Equals(input.Pickup))
                ) && 
                (
                    Delivery == input.Delivery ||
                    (Delivery != null &&
                    Delivery.Equals(input.Delivery))
                ) && 
                (
                    Size == input.Size ||
                    Size != null &&
                    Size.SequenceEqual(input.Size)
                ) && 
                (
                    RequiredSkills == input.RequiredSkills ||
                    RequiredSkills != null &&
                    RequiredSkills.SequenceEqual(input.RequiredSkills)
                ) && 
                (
                    AllowedVehicles == input.AllowedVehicles ||
                    AllowedVehicles != null &&
                    AllowedVehicles.SequenceEqual(input.AllowedVehicles)
                ) && 
                (
                    DisallowedVehicles == input.DisallowedVehicles ||
                    DisallowedVehicles != null &&
                    DisallowedVehicles.SequenceEqual(input.DisallowedVehicles)
                ) && 
                (
                    MaxTimeInVehicle == input.MaxTimeInVehicle ||
                    (MaxTimeInVehicle != null &&
                    MaxTimeInVehicle.Equals(input.MaxTimeInVehicle))
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
                if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                if (Priority != null)
                    hashCode = hashCode * 59 + Priority.GetHashCode();
                if (Pickup != null)
                    hashCode = hashCode * 59 + Pickup.GetHashCode();
                if (Delivery != null)
                    hashCode = hashCode * 59 + Delivery.GetHashCode();
                if (Size != null)
                    hashCode = hashCode * 59 + Size.GetHashCode();
                if (RequiredSkills != null)
                    hashCode = hashCode * 59 + RequiredSkills.GetHashCode();
                if (AllowedVehicles != null)
                    hashCode = hashCode * 59 + AllowedVehicles.GetHashCode();
                if (DisallowedVehicles != null)
                    hashCode = hashCode * 59 + DisallowedVehicles.GetHashCode();
                if (MaxTimeInVehicle != null)
                    hashCode = hashCode * 59 + MaxTimeInVehicle.GetHashCode();
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