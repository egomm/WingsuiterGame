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

	// Property to calculate the minimum height based on the height multiplier and curve
	public float minHeight 
	{
		get 
		{
			return heightMultiplier * heightCurve.Evaluate(0);
		}
	}

    // Property to calculate the maximum height based on the height multiplier and curve
    public float maxHeight 
	{
		get 
		{
			return heightMultiplier * heightCurve.Evaluate(1);
		}
	}

	#if UNITY_EDITOR

	// Validates the noise settings and invokes the base class validation (ie. from UpdatableData)
	protected override void OnValidate() {
		noiseSettings.ValidateValues();
		base.OnValidate();
	}
	#endif

}
