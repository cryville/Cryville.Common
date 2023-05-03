using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
using UnityEngine.Rendering;
#endif

namespace Cryville.Common.Unity {
	/// <summary>
	/// A worker that performs network tasks in the background.
	/// </summary>
	/// <remarks>
	/// It is required to call <see cref="TickBackgroundTasks" /> every tick to keep the worker working.
	/// </remarks>
	public class NetworkTaskWorker {
		bool suspended;
		NetworkTask currentNetworkTask;
		readonly Queue<NetworkTask> networkTasks = new Queue<NetworkTask>();

		/// <summary>
		/// Current queued task count.
		/// </summary>
		public int TaskCount { get { return networkTasks.Count; } }
		
		/// <summary>
		/// Submits a new network task.
		/// </summary>
		/// <param name="task">The task.</param>
		public void SubmitNetworkTask(NetworkTask task) {
			networkTasks.Enqueue(task);
		}

		/// <summary>
		/// Ticks the worker.
		/// </summary>
		/// <returns>The status of the worker.</returns>
		public WorkerStatus TickBackgroundTasks() {
			if (suspended) return WorkerStatus.Suspended;
			if (currentNetworkTask != null) {
				if (currentNetworkTask.Cancelled) currentNetworkTask = null;
				else if (currentNetworkTask.Done()) currentNetworkTask = null;
			}
			while (networkTasks.Count > 0 && currentNetworkTask == null) {
				var task = networkTasks.Dequeue();
				if (task.Cancelled) continue;
				currentNetworkTask = task;
				currentNetworkTask.Start();
			}
			return currentNetworkTask == null ? WorkerStatus.Idle : WorkerStatus.Working;
		}

		/// <summary>
		/// Cancels the current working task (if present) and suspends all background tasks.
		/// </summary>
		public void SuspendBackgroundTasks() {
			suspended = true;
			if (currentNetworkTask != null) {
				currentNetworkTask.Cancel();
				currentNetworkTask = null;
			}
		}

		/// <summary>
		/// Resumes background tasks.
		/// </summary>
		public void ResumeBackgroundTasks() {
			suspended = false;
		}
	}

	/// <summary>
	/// Status of a <see cref="NetworkTaskWorker" />.
	/// </summary>
	public enum WorkerStatus {
		/// <summary>
		/// The worker is not working nor suspended.
		/// </summary>
		Idle,
		/// <summary>
		/// The worker is working on a task.
		/// </summary>
		Working,
		/// <summary>
		/// The worker is suspended.
		/// </summary>
		Suspended,
	}

	/// <summary>
	/// A network task.
	/// </summary>
	public abstract class NetworkTask {
		protected NetworkTask(string uri) {
			Uri = uri;
		}

		/// <summary>
		/// The URI of the resource.
		/// </summary>
		public string Uri { get; private set; }

		/// <summary>
		/// Whether the task is cancelled.
		/// </summary>
		public bool Cancelled { get; private set; }

		/// <summary>
		/// Cancels the task.
		/// </summary>
		public virtual void Cancel() {
			Cancelled = true;
		}

#if UNITY_5_4_OR_NEWER
		protected UnityWebRequest www;
		
		/// <summary>
		/// Starts the task.
		/// </summary>
		public virtual void Start() {
			www = new UnityWebRequest(Uri);
			www.SendWebRequest();
		}
		/// <summary>
		/// Gets whether the task is done.
		/// </summary>
		/// <returns>Whether the task is done.</returns>
		public virtual bool Done() {
			if (!www.isDone) return false;
			return true;
		}
#else
		protected WWW www;
		/// <summary>
		/// Starts the task.
		/// </summary>
		public virtual void Start() {
			www = new WWW(Uri);
		}
		/// <summary>
		/// Gets whether the task is done.
		/// </summary>
		/// <returns>Whether the task is done.</returns>
		public virtual bool Done() {
			if (!www.isDone) return false;
			return true;
		}
#endif
	}
	/// <summary>
	/// A <see cref="NetworkTask" /> that loads a texture.
	/// </summary>
	public class LoadTextureTask : NetworkTask {
		/// <summary>
		/// Creates an instance of the <see cref="LoadTextureTask" /> class.
		/// </summary>
		/// <param name="uri">The URI of the resource.</param>
		/// <param name="callback">The callback function upon load complete.</param>
		public LoadTextureTask(string uri, Action<bool, Texture2D> callback) : base(uri) {
			Callback = callback;
		}

		/// <summary>
		/// The callback function upon load complete.
		/// </summary>
		public Action<bool, Texture2D> Callback { get; private set; }

#if UNITY_5_4_OR_NEWER
		DownloadHandlerTexture handler;
		/// <inheritdoc />
		public override void Start() {
			handler = new DownloadHandlerTexture();
			www = new UnityWebRequest(Uri, "GET", handler, null);
			www.SendWebRequest();
		}
		/// <inheritdoc />
		public override bool Done() {
			if (!www.isDone) return false;
			if (handler.isDone && handler.texture != null) {
				var buffer = handler.texture;
				var result = new Texture2D(buffer.width, buffer.height, buffer.format, true);
				if (SystemInfo.copyTextureSupport.HasFlag(CopyTextureSupport.Basic)) {
					Graphics.CopyTexture(buffer, 0, 0, result, 0, 0);
				}
				else {
					result.LoadImage(handler.data);
				}
				result.Apply(true, true);
				Texture2D.Destroy(buffer);
				Callback(true, result);
				// Callback(true, buffer);
			}
			else {
				Callback(false, null);
			}
			www.Dispose();
			handler.Dispose();
			return true;
		}
#else
		/// <inheritdoc />
		public override bool Done() {
			if (!www.isDone) return false;
			bool succeeded = string.IsNullOrEmpty(www.error);
			if (succeeded) {
				var buffer = www.texture;
				/*var result = new Texture2D(buffer.width, buffer.height, buffer.format, true);
				result.SetPixels(buffer.GetPixels());
				result.Apply(true, true);
				Texture2D.Destroy(buffer);
				Callback(true, result);*/
				Callback(true, buffer);
			}
			else Callback(false, null);
			return true;
		}
#endif
	}
}
