using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Uncover Random Token")]
public class UncoverToken : MissionBase {

	public int m_numTokens = 1;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		if (a.m_success) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<TokenSlot> revealedTokens = new List<TokenSlot> ();

			for (int i = 0; i < m_numTokens; i++) {
				List<TokenSlot> hiddenTokens = new List<TokenSlot> ();

				foreach (TokenSlot aT in r.assetTokens) {

					if (aT.m_state == TokenSlot.State.Hidden) {
						hiddenTokens.Add (aT);
					}
				}

				foreach (TokenSlot p in r.policyTokens) {

					if (p.m_state == TokenSlot.State.Hidden) {
						hiddenTokens.Add (p);
					}
				}

				foreach (TokenSlot c in r.controlTokens) {

					if (c.m_state == TokenSlot.State.Hidden) {
						hiddenTokens.Add (c);
					}
				}

				if (hiddenTokens.Count > 0) {
					TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (TokenBase.State.Revealed);
					tB.m_state = TokenSlot.State.Revealed;
					TokenBase b = tB.GetBaseToken ();
					revealedTokens.Add (tB);
				
				}

			}

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			foreach (TokenSlot ts in revealedTokens) {
				TokenBase tb = ts.GetBaseToken ();
				t.m_resultsText += "\n" + tb.m_name.ToUpper () + " is revealed!";
			}
//			t.m_resultsText += "\n" + completionChance.ToString ();
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
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (TokenSlot a in r.assetTokens) {

				if (a.m_state == TokenSlot.State.Hidden) {
					return true;
				}
			}

			foreach (TokenSlot p in r.policyTokens) {

				if (p.m_state == TokenSlot.State.Hidden) {
					return true;
				}
			}

			foreach (TokenSlot c in r.controlTokens) {

				if (c.m_state == TokenSlot.State.Hidden) {
					return true;
				}
			}
		}

		return false;
	}
}
