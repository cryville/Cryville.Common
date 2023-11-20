using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;

namespace Cryville.Common.Logging {
	/// <summary>
	/// A logger listener.
	/// </summary>
	public abstract class LoggerListener : IDisposable {
		/// <summary>
		/// Closes the logger listener and cleans up all the resources.
		/// </summary>
		/// <param name="disposing">Whether to clean up managed resources.</param>
		protected virtual void Dispose(bool disposing) { }
		/// <summary>
		/// Closes the logger listener.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Handles an incoming log.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">The message.</param>
		protected internal abstract void OnLog(int level, string category, string message);
		/// <summary>
		/// Handles an incoming log.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">An array of <see cref="char" /> containing the message.</param>
		/// <param name="index">A zero-based index of the first character of the message within <paramref name="message" />.</param>
		/// <param name="length">The length of the message.</param>
		protected internal virtual unsafe void OnLog(int level, string category, char[] message, int index, int length) {
			fixed (char* ptr = message) {
				OnLog(level, category, ptr + index, length);
			}
		}
		/// <summary>
		/// Handles an incoming log.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="category">The category.</param>
		/// <param name="message">A pointer to the first character of the message.</param>
		/// <param name="length">The length of the message.</param>
		protected internal virtual unsafe void OnLog(int level, string category, char* message, int length)
			=> OnLog(level, category, new string(message, 0, length));
	}

	/// <summary>
	/// A <see cref="LoggerListener" /> that calls a callback function on log.
	/// </summary>
	public class InstantLoggerListener : LoggerListener {
		/// <summary>
		/// Occurs when a log is logged to the logger.
		/// </summary>
		[SuppressMessage("Design", "CA1003")]
		public event LogHandler? Log;

		/// <inheritdoc />
		protected internal override void OnLog(int level, string category, string message) {
			Log?.Invoke(level, category, message);
		}
	}

	/// <summary>
	/// A <see cref="LoggerListener" /> that buffers the logs for enumeration.
	/// </summary>
	public class BufferedLoggerListener : LoggerListener {
		readonly Queue<LogEntry> _buffer = new();
		record struct LogEntry(int Level, string Category, string Message);

		/// <inheritdoc />
		protected internal override void OnLog(int level, string category, string message) {
			lock (_buffer) {
				_buffer.Enqueue(new LogEntry(level, category, message));
			}
		}
		/// <summary>
		/// Enumerates the buffered logs.
		/// </summary>
		/// <param name="callback">The callback function to receive the logs.</param>
		public void Enumerate(LogHandler callback) {
			lock (_buffer) {
				while (_buffer.Count > 0) {
					var i = _buffer.Dequeue();
					callback?.Invoke(i.Level, i.Category, i.Message);
				}
			}
		}
	}

	/// <summary>
	/// A <see cref="LoggerListener" /> that writes logs into a stream.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="encoding">The encoding.</param>
	public class StreamLoggerListener(Stream stream, Encoding encoding) : LoggerListener {
		/// <summary>
		/// Creates an instance of the <see cref="StreamLoggerListener" /> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public StreamLoggerListener(Stream stream) : this(stream, new UTF8Encoding(false, true)) { }

		readonly object _syncRoot = new();
		/// <inheritdoc />
		protected internal override void OnLog(int level, string category, string message) {
			lock (_syncRoot) {
				var buf = encoding.GetBytes(string.Format(CultureInfo.InvariantCulture, "[{0:O}] [{1}] <{2}> {3}", DateTime.UtcNow, level, category, message));
				stream.Write(buf, 0, buf.Length);
			}
		}
	}

	/// <summary>
	/// Represents the method that will handle a log.
	/// </summary>
	/// <param name="level">The severity level.</param>
	/// <param name="category">The category.</param>
	/// <param name="message">The message.</param>
	public delegate void LogHandler(int level, string category, string message);
}
