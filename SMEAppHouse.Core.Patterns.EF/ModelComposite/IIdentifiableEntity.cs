namespace SMEAppHouse.Core.Patterns.EF.ModelComposite
{
    public interface IIdentifiableEntity<TPk> : IEntity
    {
        TPk Id { get; set; }
    }
}
