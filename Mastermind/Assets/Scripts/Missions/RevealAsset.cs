using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Reveal Asset")]
public class RevealAsset : MissionBase {

	public override void CompleteMission (Organization.ActiveMission a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a.m_mission, a.m_region, a.m_henchmen);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<Region.TokenSlot> hiddenTokens = new List<Region.TokenSlot> ();

			foreach (Region.TokenSlot t in r.assetTokens) {

				if (t.m_state == Region.TokenSlot.State.Hidden) {
					hiddenTokens.Add (t);
				}
			}

			if (hiddenTokens.Count > 0) {
				Region.TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (Region.TokenSlot.State.Revealed);
				tB.m_state = Region.TokenSlot.State.Revealed;

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + tB.m_assetToken.m_name.ToUpper() + " is revealed!";
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
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionRequest != null && GameManager.instance.currentMissionRequest.m_region != null) {

			Region r = GameManager.instance.currentMissionRequest.m_region;

			foreach (Region.TokenSlot t in r.assetTokens) {

				if (t.m_state == Region.TokenSlot.State.Hidden) {
					return true;
				}
			}
		}

		return false;
	}
}
