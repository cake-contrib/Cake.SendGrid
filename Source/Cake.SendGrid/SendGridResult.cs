﻿using Cake.Core.Annotations;
using System.Text;

namespace Cake.SendGrid
{
	/// <summary>
	/// The result of SendGridProvider API post.
	/// </summary>
	[CakeAliasCategory("SendGrid")]
	public sealed class SendGridResult
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SendGridResult"/> class.
		/// </summary>
		/// <param name="ok">Indicating success or failure.</param>
		/// <param name="messageId">The unique ID of the sent email.</param>
		/// <param name="timeStamp">Timestamp of the message.</param>
		/// <param name="error">Error message on failure.</param>
		public SendGridResult(bool ok, string messageId, string timeStamp, string error)
		{
			Ok = ok;
			MessageId = messageId;
			TimeStamp = timeStamp;
			Error = error;
		}

		/// <summary>
		/// Gets a value indicating whether success or failure, <see cref="Error"/> for info on failure.
		/// </summary>
		public bool Ok { get; private set; }

		/// <summary>
		/// Gets a value indicating whether success or failure, <see cref="Error"/> for info on failure.
		/// </summary>
		public string MessageId { get; private set; }

		/// <summary>
		/// Gets the Timestamp of the message.
		/// </summary>
		public string TimeStamp { get; private set; }

		/// <summary>
		/// Gets the Error message on failure.
		/// </summary>
		public string Error { get; private set; }

		/// <summary>
		/// Convert this instance of value to a string representation.
		/// </summary>
		/// <returns>The complete string representation of the SendGridResult.</returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("{ Ok = ");
			builder.Append(Ok);
			builder.Append(", MessageId = ");
			builder.Append(MessageId);
			builder.Append(", TimeStamp = ");
			builder.Append(TimeStamp);
			builder.Append(", Error = ");
			builder.Append(Error);
			builder.Append(" }");
			return builder.ToString();
		}
	}
}
