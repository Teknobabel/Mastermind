using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Launch Into Orbit")]
public class LaunchIntoOrbit : MissionBase {

	public List<Asset> m_requiredAssets;

	public List<Asset> m_launchableAssets;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			GameManager.instance.game.player.LaunchAssetIntoOrbit (a.m_assetInFocus);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission complete!";
			t.m_resultsText += "\n" + a.m_assetInFocus.m_name.ToUpper() + " is now in orbit.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if the player is at their base, has all necessary assets, and asset in focus is launchable

		if (GameManager.instance.currentMissionWrapper.m_region.id == GameManager.instance.game.player.homeRegion.id && m_launchableAssets.Contains(GameManager.instance.currentMissionWrapper.m_assetInFocus)) {
			
			// make sure player has all required assets

			foreach (Asset a in m_requiredAssets) {

				if (!GameManager.instance.game.player.currentAssets.Contains (a) && !GameManager.instance.game.player.orgBase.m_currentAssets.Contains(a)) {

					return false;
				}
			}

			return true;
		}

		return false;
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_assetInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_assetInFocus.m_name + "</size>";
		}

		return s;
	}
}
