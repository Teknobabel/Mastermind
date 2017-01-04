using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Reveal Control Token")]
public class RevealControl : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<TokenSlot> hiddenTokens = new List<TokenSlot> ();

			foreach (TokenSlot c in r.controlTokens) {

				if (c.m_state == TokenSlot.State.Hidden) {
					hiddenTokens.Add (c);
				}
			}

			if (hiddenTokens.Count > 0) {
				TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (TokenBase.State.Revealed);
				tB.m_state = TokenSlot.State.Revealed;

				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + tB.m_controlToken.m_name.ToUpper() + " is revealed!";
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

	public override bool IsValid ()
	{
		bool hasPreRequisites = base.IsValid ();	

		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (TokenSlot c in r.controlTokens) {

				if (c.m_state == TokenSlot.State.Hidden && hasPreRequisites) {
					return true;
				}
			}
		}

		return false;
	}
}
