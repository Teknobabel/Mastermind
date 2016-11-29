using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Uncover Remote Intel")]
public class UncoverRemoteIntel : MissionBase {

	public Asset m_requiredAsset;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
		a.m_success = true;
		if (a.m_success) {

			foreach (Region r in GameManager.instance.game.regions) {

				foreach (TokenSlot ts in r.assetTokens) {

					if (ts.m_assetToken == GameManager.instance.m_intel && ts.m_state == TokenSlot.State.Hidden) {

						// uncover hidden intel

						ts.m_state = TokenSlot.State.Revealed;

						TurnResultsEntry t = new TurnResultsEntry ();
						t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission complete!";
						t.m_resultsText += "\nIntel discovered in " + r.regionName.ToUpper() + "!";
						t.m_resultType = GameEvent.Henchmen_MissionCompleted;
						GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

						return;
					}
				}
			}

		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission complete!";
			t.m_resultsText += "\nNothing found.";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		// valid if player has control room, intel is hidden in world, region scope is player base

		if (GameManager.instance.game.intelInPlay.Count > 0 && GameManager.instance.currentMissionWrapper.m_region.id == GameManager.instance.game.player.homeRegion.id && 
			GameManager.instance.game.player.orgBase.m_currentAssets.Contains(m_requiredAsset)) {

			// make sure 1 intel is still hidden

			foreach (Region r in GameManager.instance.game.regions) {

				foreach (TokenSlot ts in r.assetTokens) {

					if (ts.m_assetToken == GameManager.instance.m_intel && ts.m_state == TokenSlot.State.Hidden) {

						return true;
					}
				}
			}
		}

		return false;
	}
}
