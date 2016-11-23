using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Remove Floor")]
public class RemoveFloor : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			// remove asset and set floor to empty

			Asset asset = a.m_floorInFocus.m_installedUpgrade;

			GameManager.instance.game.player.orgBase.RemoveAsset (a.m_floorInFocus.m_floorNumber);

			if (GameManager.instance.game.player.currentAssets.Contains (asset)) {
				GameManager.instance.game.player.currentAssets.Remove (asset);
			}

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + asset.m_name.ToUpper () + " is removed from the Base.";
//			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
//			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// Valid if we are currently looking at a base floor with an upgrade installed

		if (GameManager.instance.currentMissionWrapper.m_region == GameManager.instance.game.player.homeRegion && GameManager.instance.currentMissionWrapper.m_floorInFocus != null &&
			GameManager.instance.currentMissionWrapper.m_floorInFocus.m_floorState == Base.FloorState.Occupied) 
		{
			return true;
		}

		return false;
	}
}
