using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Capture Intel")]
public class CaptureIntel : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool success = WasMissionSuccessful (completionChance);

		if (success) {

			// remove intel asset from region

			foreach (Region.TokenSlot ts in a.m_region.assetTokens) {
				if (ts.m_state == Region.TokenSlot.State.Revealed && ts.m_assetToken == GameManager.instance.m_intel) {

					a.m_region.RemoveAssetToken (ts);
					GameManager.instance.game.IntelCaptured ();

					break;
				}
			}

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + "INTEL HAS BEEN RECOVERED BEFORE AGENTS COULD INTERCEPT.";
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

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (Region.TokenSlot ts in r.assetTokens) {
				if (ts.m_state == Region.TokenSlot.State.Revealed && ts.m_assetToken == GameManager.instance.m_intel) {
					return true;
				}
			}
		}
		return false;
	}
}
