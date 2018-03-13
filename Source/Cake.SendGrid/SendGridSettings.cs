using Cake.Core.Annotations;

namespace Cake.SendGrid.Email
{
	/// <summary>
	/// Class that lets you override default API settings
	/// </summary>
	[CakeAliasCategory("SendGrid")]
	public sealed class SendGridSettings
	{
		/// <summary>
		/// Gets or sets the ApiKey used for authentication.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Gets or sets the Optional flag for if should throw exception on failure
		/// </summary>
		public bool? ThrowOnFail { get; set; }
	}
}
