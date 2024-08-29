using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
public class UpdatableData : ScriptableObject 
{
	// Event triggered when the values are updated (ie. the values in the Procedural Generation Settings folder)
	public event System.Action OnValuesUpdated;
	public bool autoUpdate;

	#if UNITY_EDITOR

	/// <summary>
	/// Called when a scriptable object is loaded or a value in the inspector is changed (ie. the values in the folder).
	/// If auto update is enabled, the NotifyOfUpdatedValues method is scheduled to be called.
	/// </summary>
	protected virtual void OnValidate() 
	{
		if (autoUpdate) 
		{
			UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
		}
	}

	/// <summary>
	/// This method notifies subscribers (listeners) that the values have been updated.
	/// The method is removed from the update event once called.
	/// </summary>
	public void NotifyOfUpdatedValues() 
	{
		UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
		if (OnValuesUpdated != null) 
		{
			OnValuesUpdated ();
		}
	}

	#endif

}
