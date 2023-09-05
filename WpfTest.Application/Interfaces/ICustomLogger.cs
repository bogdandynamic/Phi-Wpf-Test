using System;

namespace WpfTest.Application.Interfaces
{
	/// <summary>
	/// This interface is created for brevity. No need to add Microsoft.Extensions.Logging into the project.
	/// The methods are self explanatory.
	/// </summary>
	public interface ICustomLogger
	{
		void Info(string message);

		void Warning(string message);

		void Error(Exception? exception, string message);
	}
}
