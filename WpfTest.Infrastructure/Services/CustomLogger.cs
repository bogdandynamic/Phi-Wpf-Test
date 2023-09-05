using System;
using WpfTest.Application.Interfaces;

namespace WpfTest.Infrastructure.Services
{
	/// <summary>
	/// This implementation of the CustomLogger will write only to the VS Warning Window
	/// </summary>
	public class CustomLogger : ICustomLogger
	{
		public void Info(string message)
		{
			System.Diagnostics.Debug.WriteLine($"INFO: {message}");
		}

		public void Warning(string message)
		{
			System.Diagnostics.Debug.WriteLine($"WARN: {message}");
		}

		public void Error(Exception? exception, string message)
		{
			System.Diagnostics.Debug.WriteLine($"ERR: {message}, Exception: {exception?.Message ?? "UNKNOWN"}");
		}
	}
}
