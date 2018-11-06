namespace SMEAppHouse.Core.AppMgt.AppCfgs.Validator
{
    /// <summary>
    /// https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-asp-net-core/
    /// </summary>
    public interface IValidatable
    {
        void Validate();
    }
}