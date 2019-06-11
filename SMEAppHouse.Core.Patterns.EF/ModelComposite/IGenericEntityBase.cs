namespace SMEAppHouse.Core.Patterns.EF.ModelComposite
{
    public interface IGenericEntityBase<TPk> : IEntityBase
    {
        TPk Id { get; set; }
    }
}
