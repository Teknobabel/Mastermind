using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Zombify Control Token")]
public class ZombifyToken : MissionBase {

	public TokenBase m_token;
	public OmegaPlanData m_omegaPlanData;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			List<TokenSlot> validTokens = new List<TokenSlot> ();

			foreach (TokenSlot ts in a.m_region.controlTokens) {

				if (ts.m_state == TokenSlot.State.Revealed && ts.m_controlToken == m_token && !ts.m_effects.Contains(TokenSlot.Status.Zombified)) {
					validTokens.Add (ts);
				}
			}

			if (validTokens.Count > 0) {

				int rand = Random.Range (0, validTokens.Count);

				TokenSlot token = validTokens [rand];

//				token.m_status = TokenSlot.Status.Zombified;
				token.m_effects.Add(TokenSlot.Status.Zombified);

				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + token.m_controlToken.m_name.ToUpper () + " has been Zombified!";
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
		if (!base.IsValid ()) { return false;}		bool hasOmegaPlan = false;

		foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {
			if (op.opName == m_omegaPlanData.m_name && op.state == OmegaPlan.State.Revealed) {
				hasOmegaPlan = true;
				break;
			}
		}

		if (hasOmegaPlan && GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (TokenSlot ts in r.controlTokens) {

				if (ts.m_state == TokenSlot.State.Revealed && ts.m_controlToken == m_token && !ts.m_effects.Contains(TokenSlot.Status.Zombified)) {
					return true;
				}
			}
		}

		return false;
	}
}
