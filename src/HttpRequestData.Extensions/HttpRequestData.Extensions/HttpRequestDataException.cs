using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Runtime.Serialization;

namespace TaleLearnCode
{

	/// <summary>
	/// Exception that is thrown when there is an error processing a <see cref="HttpRequestData"/>
	/// </summary>
	/// <seealso cref="Exception" />
	public class HttpRequestDataException : Exception
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestDataException"/> class.
		/// </summary>
		public HttpRequestDataException() : base() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestDataException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public HttpRequestDataException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestDataException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public HttpRequestDataException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestDataException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		public HttpRequestDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }

	}

}