using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Steal Asset")]
public class StealAsset : MissionBase {

	public override void CompleteMission (Organization.ActiveMission a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a.m_mission, a.m_region, a.m_henchmen);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {

			// find a revealed non-empty asset token
			foreach (Region.TokenSlot at in a.m_region.assetTokens) {

				if (at.m_state == Region.TokenSlot.State.Revealed) {

					// remove it from region and add to player bank

					Asset asset = at.m_assetToken.m_asset;

					GameManager.instance.game.player.AddAsset (at.m_assetToken.m_asset);

					a.m_region.RemoveAssetToken (at);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\n" + GameManager.instance.game.player.orgName.ToUpper() + " GAINS A " + asset.m_name.ToUpper() + " ASSET.";
					t.m_resultsText += "\n" + completionChance.ToString ();
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

					break;
				}
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
		// valid if there is a revealed, non empty asset token in the region

		if (GameManager.instance.currentMissionRequest != null && GameManager.instance.currentMissionRequest.m_region != null) {

			Region r = GameManager.instance.currentMissionRequest.m_region;

			foreach (Region.TokenSlot a in r.assetTokens) {

				if (a.m_state == Region.TokenSlot.State.Revealed) {

					return true;
				}
			}
		}

		return false;
	}
}
