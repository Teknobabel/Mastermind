using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Capture Intel")]
public class CaptureIntel : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// remove intel asset from region

			foreach (TokenSlot ts in a.m_region.assetTokens) {
				if (ts.m_state == TokenSlot.State.Revealed && ts.m_assetToken == GameManager.instance.m_intel) {

					a.m_region.RemoveAssetToken (ts);
					GameManager.instance.game.IntelCaptured ();

					break;
				}
			}

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + "INTEL HAS BEEN RECOVERED BEFORE AGENTS COULD INTERCEPT.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}
		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (TokenSlot ts in r.assetTokens) {
				if (ts.m_state == TokenSlot.State.Revealed && ts.m_assetToken == GameManager.instance.m_intel) {
					return true;
				}
			}
		}
		return false;
	}
}
