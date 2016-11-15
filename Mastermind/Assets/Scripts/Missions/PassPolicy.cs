using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Pass Policy")]
public class PassPolicy : MissionBase {

	public PolicyToken m_policy;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			a.m_region.AddPolicytoken (m_policy, a.m_tokenInFocus);

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			t.m_resultsText += "\n" + m_policy.m_name.ToUpper() + " has passed!";
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
		// valid if the token in focus is an empty policy token

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_tokenInFocus != null) {

			Region.TokenSlot ts = GameManager.instance.currentMissionWrapper.m_tokenInFocus;

			if (ts.m_type == Region.TokenSlot.TokenType.Policy && ts.m_state == Region.TokenSlot.State.None) {
				return true;
			}
		}

		return false;
	}
}
