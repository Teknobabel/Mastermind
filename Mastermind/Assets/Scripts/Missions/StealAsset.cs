using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class StealAsset : MissionBase {

	public override void CompleteMission (Organization.ActiveMission a)
	{
		Debug.Log ("Processing Steal Asset");

		TurnResultsEntry t = new TurnResultsEntry ();
//		t.m_resultsText = h.henchmenName.ToUpper() + " arrives in " + a.m_region.regionName.ToUpper();
		t.m_resultsText = "Steal Asset mission is a success!";
		t.m_resultType = GameEvent.Henchmen_ArriveInRegion;
		GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
	}

	public override bool IsValid ()
	{
		// valid if there is a revealed, non empty asset token in the region

		if (GameManager.instance.currentMissionRequest != null && GameManager.instance.currentMissionRequest.m_region != null) {

			Region r = GameManager.instance.currentMissionRequest.m_region;

			foreach (Region.TokenSlot a in r.assetTokens) {

			}
		}

		return false;
	}
}
