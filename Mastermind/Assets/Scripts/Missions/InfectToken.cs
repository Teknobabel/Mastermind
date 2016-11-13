using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Infect Control Token")]
public class InfectToken : MissionBase {

	public TokenBase m_token;
	public OmegaPlanData m_omegaPlanData;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {

			List<Region.TokenSlot> validTokens = new List<Region.TokenSlot> ();

			foreach (Region.TokenSlot ts in a.m_region.controlTokens) {

				if (ts.m_state == Region.TokenSlot.State.Revealed && ts.m_controlToken == m_token && !ts.m_effects.Contains(Region.TokenSlot.Status.Infected)) {
					validTokens.Add (ts);
				}
			}

			if (validTokens.Count > 0) {

				int rand = Random.Range (0, validTokens.Count);

				Region.TokenSlot token = validTokens [rand];

//				token.m_status = Region.TokenSlot.Status.Infected;
				token.m_effects.Add(Region.TokenSlot.Status.Infected);

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + token.m_controlToken.m_name.ToUpper () + " has been Infected!";
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

		if (hasOmegaPlan && GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (Region.TokenSlot ts in r.controlTokens) {

				if (ts.m_state == Region.TokenSlot.State.Revealed && ts.m_controlToken == m_token && !ts.m_effects.Contains(Region.TokenSlot.Status.Infected)) {
					return true;
				}
			}
		}

		return false;
	}
}
