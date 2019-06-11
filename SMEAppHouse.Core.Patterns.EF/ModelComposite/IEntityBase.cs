using System;

namespace SMEAppHouse.Core.Patterns.EF.ModelComposite
{
    public interface IEntityBase
    {
        int? Ordinal { get; set; }
        bool? IsNotActive { get; set; }
        DateTime? DateCreated { get; set; }
        DateTime? DateRevised { get; set; }
        string CreatedBy { get; set; }
        string RevisedBy { get; set; }
    }
}