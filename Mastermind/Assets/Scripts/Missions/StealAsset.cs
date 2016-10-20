using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class StealAsset : MissionBase {

	public override void CompleteMission ()
	{
		Debug.Log ("Processing Steal Asset");
	}

	public override bool IsValid ()
	{
		return true;
	}
}
