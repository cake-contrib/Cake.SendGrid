using Cake.Core;
using Cake.Core.Annotations;
using System;

[assembly: CLSCompliant(true)]

namespace Cake.SendGrid
{
	/// <summary>
	/// <para>Contains aliases related to <see href="https://sendgrid.com/">SendGrid</see>.</para>
	/// <para>
	/// In order to use the commands for this addin, you will need to include the following in your build.cake file to download and
	/// reference from NuGet.org:
	/// <code>
	/// #addin Cake.SendGrid
	/// </code>
	/// </para>
	/// </summary>
	[CakeAliasCategory("SendGrid")]
	public static class SendGridAliases
	{
		/// <summary>
		/// Gets a <see cref="SendGridProvider"/> instance that can be used for communicating with SendGridProvider API.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A <see cref="SendGridProvider"/> instance.</returns>
		[CakePropertyAlias(Cache = true)]
		[CakeNamespaceImport("Cake.SendGrid.Email")]
		public static SendGridProvider SendGrid(this ICakeContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			return new SendGridProvider(context);
		}
	}
}
