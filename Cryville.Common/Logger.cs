using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Cryville.Common {
	/// <summary>
	/// A logger.
	/// </summary>
	public abstract class Logger {
		static readonly Dictionary<string, Logger> Instances = new Dictionary<string, Logger>();
		static readonly Dictionary<string, StreamWriter> Files = new Dictionary<string, StreamWriter>();
		static string logPath = null;
		/// <summary>
		/// Sets the path where the log files shall be stored.
		/// </summary>
		/// <param name="path">The path.</param>
		public static void SetLogPath(string path) {
			logPath = path;
			var dir = new DirectoryInfo(path);
			if (!dir.Exists) dir.Create();
		}
		/// <summary>
		/// Logs to the specified logger.
		/// </summary>
		/// <param name="key">The key of the logger.</param>
		/// <param name="level">The severity level.</param>
		/// <param name="module">The module that is logging.</param>
		/// <param name="format">The format string.</param>
		/// <param name="args">The arguments for formatting.</param>
		public static void Log(string key, int level, string module, string format, params object[] args) {
			if (!Instances.ContainsKey(key)) return;
			Instances[key].Log(level, module, string.Format(format, args));
			if (Files.ContainsKey(key)) Files[key].WriteLine("[{0:O}] [{1}] <{2}> {3}", DateTime.UtcNow, level, module, string.Format(format, args));
		}
		/// <summary>
		/// Adds a created logger to the shared logger manager.
		/// </summary>
		/// <param name="key">The key of the logger.</param>
		/// <param name="logger">The logger.</param>
		public static void Create(string key, Logger logger) {
			Instances[key] = logger;
			if (logPath != null) {
				Files[key] = new StreamWriter(logPath + "/" + ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString(CultureInfo.InvariantCulture) + "-" + key + ".log") {
					AutoFlush = true
				};
			}
		}
		/// <summary>
		/// Closes all loggers and related file streams.
		/// </summary>
		public static void Close() {
			Instances.Clear();
			foreach (var f in Files) f.Value.Dispose();
			Files.Clear();
		}
		/// <summary>
		/// Logs to the logger.
		/// </summary>
		/// <param name="level">The severity level.</param>
		/// <param name="module">The module that is logging.</param>
		/// <param name="msg">The message.</param>
		public virtual void Log(int level, string module, string msg) { }
	}

	/// <summary>
	/// A <see cref="Logger" /> that calls a callback function on log.
	/// </summary>
	public class InstantLogger : Logger {
		readonly Action<int, string, string> callback;
		/// <summary>
		/// Creates an instance of the <see cref="InstantLogger" /> class.
		/// </summary>
		/// <param name="callback">The callback function.</param>
		/// <exception cref="ArgumentNullException"><paramref name="callback" /> is <see langword="null" />.</exception>
		public InstantLogger(Action<int, string, string> callback) {
			if (callback == null)
				throw new ArgumentNullException("callback");
			this.callback = callback;
		}
		/// <inheritdoc />
		public override void Log(int level, string module, string msg) {
			base.Log(level, module, msg);
			callback(level, module, msg);
		}
	}

	/// <summary>
	/// A <see cref="Logger" /> that buffers the logs for enumeration.
	/// </summary>
	public class BufferedLogger : Logger {
		readonly List<LogEntry> buffer = new List<LogEntry>();
		/// <summary>
		/// Creates an instance of the <see cref="BufferedLogger" /> class.
		/// </summary>
		public BufferedLogger() { }
		/// <inheritdoc />
		public override void Log(int level, string module, string msg) {
			base.Log(level, module, msg);
			lock (buffer) {
				buffer.Add(new LogEntry(level, module, msg));
			}
		}
		/// <summary>
		/// Enumerates the buffered logs.
		/// </summary>
		/// <param name="callback">The callback function to receive the logs.</param>
		public void Enumerate(Action<int, string, string> callback) {
			lock (buffer) {
				foreach (var i in buffer) {
					callback(i.level, i.module, i.msg);
				}
			}
			buffer.Clear();
		}
	}

	struct LogEntry {
		public int level;
		public string module;
		public string msg;
		public LogEntry(int level, string module, string msg) {
			this.level = level;
			this.module = module;
			this.msg = msg;
		}
	}
}
