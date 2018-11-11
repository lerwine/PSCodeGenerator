namespace CsCodeGenerator
{
    public enum CodeClassExtensibility
    {
        /// <summary>
        /// Specifies that the class can be extended.
        /// </summary>
        Extensible,

        /// <summary>
        /// Specifies that the class cannot be extended.
        /// </summary>
        Sealed,

        /// <summary>
        /// Specifies that the class is abstract.
        /// </summary>
        Abstract
    }
}