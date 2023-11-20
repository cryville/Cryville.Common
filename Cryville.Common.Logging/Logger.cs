using System;
using System.Collections.Generic;

namespace Cryville.Common.Logging {
	/// <summary>
	/// A logger.
	/// </summary>
	public class Logger {
		readonly HashSet<LoggerListener> _listeners = [];

		/// <summary>
		/// Attaches a listener to the logger.
		/// </summary>
		/// <param name="listener">The logger listener.</param>
		public void AddListener(LoggerListener listener) {
			lock (_listeners) {
				_listeners.Add(listener);
			}
		}
		/// <summary>
		/// Detaches a listener from the logger.
		/// </summary>
		/// <param name="listener">The logger listener.</param>
		public void RemoveListener(LoggerListener listener) {
			lock (_listeners) {
				_listeners.Remove(listener);
			}
		}

		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The arguments for formatting.</param>
		public void Log(int level, string category, string format, params object[] args)
			=> Log(level, category, null, format, args);
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="provider">The format provider.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The arguments for formatting.</param>
		public void Log(int level, string category, IFormatProvider? provider, string format, params object[] args)
			=> Log(level, category, string.Format(provider, format, args));
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">The message.</param>
		public void Log(int level, string category, string message) {
			lock (_listeners) {
				foreach (var listener in _listeners)
					listener.OnLog(level, category, message);
			}
		}
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">An array of <see cref="char" /> containing the message.</param>
		public void Log(int level, string category, char[] message) => Log(level, category, message ?? throw new ArgumentNullException(nameof(message)), 0, message.Length);
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">An array of <see cref="char" /> containing the message.</param>
		/// <param name="index">A zero-based index of the first character of the message within <paramref name="message" />.</param>
		/// <param name="length">The length of the message.</param>
		public void Log(int level, string category, char[] message, int index, int length) {
			lock (_listeners) {
				foreach (var listener in _listeners)
					listener.OnLog(level, category, message, index, length);
			}
		}
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">A pointer to the first character of the message.</param>
		/// <param name="length">The length of the message.</param>
		public unsafe void Log(int level, string category, char* message, int length) {
			lock (_listeners) {
				foreach (var listener in _listeners)
					listener.OnLog(level, category, message, length);
			}
		}
	}
}
