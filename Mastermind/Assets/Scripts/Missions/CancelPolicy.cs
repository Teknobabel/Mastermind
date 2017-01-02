using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Cancel Policy")]
public class CancelPolicy : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// cancel policy

			if (a.m_tokenInFocus != null) {

				PolicyToken p = a.m_tokenInFocus.m_policyToken;

				a.m_tokenInFocus.m_region.RemovePolicyToken (a.m_tokenInFocus);

				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + p.m_name.ToUpper() + " is cancelled!";
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
				
		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_tokenInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_tokenInFocus.m_policyToken.m_name + "</size>";
		}

		return s;
	}

	public override bool IsValid ()
	{
		if (!base.IsValid ()) { return false;}		// valid if there is a control token of m_type not under player control

		if (GameManager.instance.currentMissionWrapper.m_tokenInFocus != null)
		{
			TokenSlot t = GameManager.instance.currentMissionWrapper.m_tokenInFocus;

			if (t.m_type == TokenSlot.TokenType.Policy && t.m_state == TokenSlot.State.Revealed && t.m_policyToken != null) {
				return true;
			}
		}
		return false;
	}
}
