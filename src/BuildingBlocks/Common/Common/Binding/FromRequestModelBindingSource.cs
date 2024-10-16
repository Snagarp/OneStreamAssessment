using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Binding
{
    /// <summary>
    /// Represents a custom model binding source that binds data from the request model.
    /// </summary>
    public class FromRequestModelBindingSource : BindingSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FromRequestModelBindingSource"/> class.
        /// </summary>
        public FromRequestModelBindingSource()
            : base("FromModelBinding", "FromModelBinding", isGreedy: true, isFromRequest: true) { }

        /// <summary>
        /// Determines whether the data can be accepted from the provided binding source.
        /// </summary>
        /// <param name="bindingSource">The source from which the data is being bound.</param>
        /// <returns><c>true</c> if the data can be accepted; otherwise, <c>false</c>.</returns>
        public override bool CanAcceptDataFrom(BindingSource bindingSource) =>
            bindingSource == ModelBinding;
    }
}
