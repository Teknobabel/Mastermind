using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Open Portal")]
public class OpenPortal : MissionBase {

	public List<Asset> m_sourceAssets;
	public Asset m_createdAsset;
	public OmegaPlanData m_omegaPlanData;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {
			
			bool hasAllSourceAssets = true;

			foreach (Asset asset in m_sourceAssets) {

				if (!GameManager.instance.game.player.currentAssets.Contains (asset)) {
					hasAllSourceAssets = false;
					break;
				}
			}

			if (hasAllSourceAssets) {

				GameManager.instance.game.player.AddAsset (m_createdAsset);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + completionChance.ToString ();
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}

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
		bool hasOmegaPlan = false;

		foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {
			if (op.opName == m_omegaPlanData.m_name && op.state == OmegaPlan.State.Revealed) {
				hasOmegaPlan = true;
				break;
			}
		}

		bool hasAssets = true;

		foreach (Asset a in m_sourceAssets) {
			if (!GameManager.instance.game.player.currentAssets.Contains (a)) {
				hasAssets = false;
				break;
			}
		}

		if (hasOmegaPlan && hasAssets && GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			if (r == GameManager.instance.game.player.homeRegion) {

				return true;
			}
		}

		return false;
	}
}
