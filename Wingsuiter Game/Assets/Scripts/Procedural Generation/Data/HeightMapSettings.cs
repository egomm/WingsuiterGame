using UnityEngine;

/// <summary>
/// NOTE: This code has been adapted from a tutorial and thus the majority of this code is not my own
/// A link to the tutorial can be found here: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
/// All of the code comments are written by me based on my own understanding of this code.
/// </summary>
[CreateAssetMenu()]
public class HeightMapSettings : UpdatableData 
{
	public NoiseSettings noiseSettings;

	public float heightMultiplier;
	public AnimationCurve heightCurve;

	public float minHeight 
	{
		get 
		{
			return heightMultiplier * heightCurve.Evaluate(0);
		}
	}

	public float maxHeight 
	{
		get 
		{
			return heightMultiplier * heightCurve.Evaluate(1);
		}
	}

	#if UNITY_EDITOR

	protected override void OnValidate() {
		noiseSettings.ValidateValues();
		base.OnValidate();
	}
	#endif

}
