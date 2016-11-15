﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Missions/Unlock Omega Plan")]
public class UnlockOmegaPlan : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

				if (op.state == OmegaPlan.State.Hidden) {

					op.ChangeState (OmegaPlan.State.Revealed);

					TurnResultsEntry t = new TurnResultsEntry ();
					t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
					t.m_resultsText += "\nOMEGA PLAN: " + op.opName.ToUpper() + " is now unlocked!";
//					t.m_resultsText += "\n" + completionChance.ToString ();
					t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
					t.m_resultType = GameEvent.Henchmen_MissionCompleted;
					GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

					break;
				}
			}

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
		// valid if there is a locked Omega Plan and the current region is the Lair

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;
			if (r == GameManager.instance.game.player.homeRegion) {

				foreach (OmegaPlan op in GameManager.instance.game.player.omegaPlans) {

					if (op.state == OmegaPlan.State.Hidden) {
						return true;
					}
				}
			}
		}

		return false;
	}
}
