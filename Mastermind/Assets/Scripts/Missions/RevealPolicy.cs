﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Reveal Policy Token")]
public class RevealPolicy : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<Region.TokenSlot> hiddenTokens = new List<Region.TokenSlot> ();

			foreach (Region.TokenSlot p in r.policyTokens) {

				if (p.m_state == Region.TokenSlot.State.Hidden) {
					hiddenTokens.Add (p);
				}
			}

			if (hiddenTokens.Count > 0) {
				Region.TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (TokenBase.State.Revealed);
				tB.m_state = Region.TokenSlot.State.Revealed;

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				if (tB.m_policyToken != null) {
					t.m_resultsText += "\n" + tB.m_policyToken.m_name.ToUpper () + " is revealed!";
				} else {
					t.m_resultsText += "\n Empty Policy Slot is revealed!";
				}
//				t.m_resultsText += "\n" + completionChance.ToString ();
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
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
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (Region.TokenSlot p in r.policyTokens) {

				if (p.m_state == Region.TokenSlot.State.Hidden) {
					return true;
				}
			}
		}

		return false;
	}
}
