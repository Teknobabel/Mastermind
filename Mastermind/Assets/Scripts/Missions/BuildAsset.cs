using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Build Asset")]
public class BuildAsset : MissionBase {

	public Asset m_asset;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool success = WasMissionSuccessful (completionChance);

		if (success) {
			
			// add asset to player's inventory

			GameManager.instance.game.player.AddAsset (m_asset);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper() + " GAINS A " + m_asset.m_name.ToUpper() + " ASSET.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n" + completionChance.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		
		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null &&
			GameManager.instance.currentMissionWrapper.m_scope == m_targetType) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;
			if (r == GameManager.instance.game.player.homeRegion && !GameManager.instance.game.player.currentAssets.Contains(m_asset)) {
				return true;
			}
		}
		return false;
	}
}
