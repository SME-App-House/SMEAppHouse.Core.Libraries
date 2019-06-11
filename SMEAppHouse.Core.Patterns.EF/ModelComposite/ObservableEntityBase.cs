using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposite
{
    /// <summary>
    /// Why Datetime2? &gt; https://stackoverflow.com/a/1884088
    /// </summary>
    /// <typeparam name="TPk"></typeparam>
    public class ObservableEntityBase<TPk> : IGenericEntityBase<TPk>, INotifyPropertyChanged
    {
        private TPk _id;
        private int? _ordinal;
        private DateTime? _dateCreated = DateTime.UtcNow;
        private DateTime? _dateRevised = DateTime.UtcNow;
        private bool _isNotActive = false;

        #region constructors

        protected ObservableEntityBase()
        {
            Id = default(TPk);
            Ordinal = 0;
            IsNotActive = false;
            DateCreated = DateTime.UtcNow;
            DateRevised = DateTime.UtcNow;
        }

        #endregion

        /// <summary>
        /// Primary key of this data model
        /// </summary>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public TPk Id
        {
            get => _id;
            set
            {
                if (value.Equals(_id)) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public Type GetIdentType()
        {
            var test = default(TPk);
            return test?.GetType();
        }

        /// <summary>
        /// Applicable as order identifier of this record from among other records in the list.
        /// </summary>
        [Column(Order = 1)]
        public int? Ordinal
        {
            get => _ordinal;
            set
            {
                if (value.Equals(_ordinal)) return;
                if (value != null) _ordinal = value.Value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Date this record was created
        /// https://stackoverflow.com/a/1884088
        /// </summary>
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Column(Order = 501, TypeName = "DateTime2")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateCreated
        {
            get
            {
                if (_dateCreated != null)
                    return DateTime.SpecifyKind(_dateCreated.Value, DateTimeKind.Utc);
                return null;
            }
            set
            {
                if (value.Equals(_dateCreated)) return;
                if (value != null) _dateCreated = value.Value.ToUniversalTime();
                OnPropertyChanged();
            }
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 502)]
        [DataType(DataType.Text)]
        [StringLength(32)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Date this record was modified
        /// </summary>
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = true)]
        [Column(Order = 503, TypeName = "DateTime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateRevised
        {
            get
            {
                if (_dateRevised != null)
                    return DateTime.SpecifyKind(_dateRevised.Value, DateTimeKind.Utc);
                return null;
            }
            set
            {
                if (value.Equals(_dateRevised)) return;
                if (value != null) _dateRevised = value.Value.ToUniversalTime();
                OnPropertyChanged();
            }
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 504)]
        [DataType(DataType.Text)]
        [StringLength(32)]
        public string RevisedBy { get; set; }

        /// <summary>
        /// Used to indicate the model is active and can be used by the service operations.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 505)]
        public bool? IsNotActive
        {
            get => _isNotActive;
            set
            {
                if (value == null || value.Equals(_isNotActive)) return;
                _isNotActive = value.Value;
                OnPropertyChanged();
            }
        }


        public new virtual string ToString()
        {
            return $"Id:{Id} created: {DateCreated} revised:{DateRevised} ordinal: {Ordinal}";
        }

        public static IEnumerable<Type> GetImplementors()
        {
            var type = typeof(IGenericEntityBase<TPk>);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            return types;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.VerifyPropertyName(propertyName);
            var handler = this.PropertyChanged;
            if (handler == null) return;
            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        #endregion // INotifyPropertyChanged Members

        #region Debugging Aides

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public virtual void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null) return;
            var msg = "Invalid property name: " + propertyName;

            if (this.ThrowOnInvalidPropertyName)
                throw new Exception(msg);
            else Debug.Fail(msg);
        }

        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides    
    }
}
