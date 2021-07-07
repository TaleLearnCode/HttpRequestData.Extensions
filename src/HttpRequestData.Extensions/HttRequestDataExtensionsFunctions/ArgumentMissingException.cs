using System;
using System.Runtime.Serialization;

namespace HttRequestDataExtensionsFunctions
{

	/// <summary>
	/// Exception that is thrown when there is an error processing a <see cref="HttpRequestData"/>
	/// </summary>
	/// <seealso cref="Exception" />
	public class ArgumentMissingException : ArgumentNullException
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentMissingException"/> class.
		/// </summary>
		public ArgumentMissingException() : base() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentMissingException"/> class.
		/// </summary>
		/// <param name="paramName">The name of the parameter that caused the exception.</param>
		public ArgumentMissingException(string paramName) : base(paramName) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentMissingException"/> class.
		/// </summary>
		/// <param name="paramName">The name of the parameter that caused the exception.</param>
		/// <param name="message">A message that describes the error.</param>
		public ArgumentMissingException(string paramName, string message) : base(paramName, message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentMissingException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public ArgumentMissingException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="ArgumentMissingException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		public ArgumentMissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}

}