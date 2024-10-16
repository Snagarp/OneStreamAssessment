using Common.Exceptions;
using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception related to operations on the <see cref="Country"/> domain.
    /// </summary>
    public class CountryDomainException : Exception, IDomainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDomainException"/> class.
        /// </summary>
        public CountryDomainException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDomainException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CountryDomainException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryDomainException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CountryDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
