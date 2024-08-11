using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public class ThreadedDataRequester : MonoBehaviour 
{

	static ThreadedDataRequester instance;
	Queue<ThreadInfo> dataQueue = new Queue<ThreadInfo>();

	void Awake() 
	{
		instance = FindObjectOfType<ThreadedDataRequester> ();
	}

	public static void RequestData(Func<object> generateData, Action<object> callback) 
	{
		ThreadStart threadStart = delegate {
			instance.DataThread(generateData, callback);
		};

		new Thread (threadStart).Start();
	}

	void DataThread(Func<object> generateData, Action<object> callback) 
	{
		object data = generateData ();
		lock (dataQueue) 
		{
			dataQueue.Enqueue(new ThreadInfo (callback, data));
		}
	}
		

	void Update() 
	{
		if (dataQueue.Count > 0) 
		{
			for (int i = 0; i < dataQueue.Count; i++) 
			{
				ThreadInfo threadInfo = dataQueue.Dequeue();
				threadInfo.callback(threadInfo.parameter);
			}
		}
	}

	struct ThreadInfo 
	{
		public readonly Action<object> callback;
		public readonly object parameter;

		public ThreadInfo (Action<object> callback, object parameter)
		{
			this.callback = callback;
			this.parameter = parameter;
		}

	}
}
