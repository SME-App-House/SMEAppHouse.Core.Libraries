using System;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposite
{
    public interface IEntity
    {
        int? Ordinal { get; set; }
        bool? IsActive { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateRevised { get; set; }
        Guid? CreatedBy { get; set; }
        Guid? RevisedBy { get; set; }
    }
}