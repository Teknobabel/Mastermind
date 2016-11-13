using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Uncover Random Token")]
public class UncoverToken : MissionBase {

	public int m_numTokens = 1;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		int completionChance = CalculateCompletionPercentage (a);

		bool missionSuccess = WasMissionSuccessful (completionChance);

		if (missionSuccess) {

			// gather all hidden tokens

			Region r = a.m_region;

			List<Region.TokenSlot> revealedTokens = new List<Region.TokenSlot> ();

			for (int i = 0; i < m_numTokens; i++) {
				List<Region.TokenSlot> hiddenTokens = new List<Region.TokenSlot> ();

				foreach (Region.TokenSlot aT in r.assetTokens) {

					if (aT.m_state == Region.TokenSlot.State.Hidden) {
						hiddenTokens.Add (aT);
					}
				}

				foreach (Region.TokenSlot p in r.policyTokens) {

					if (p.m_state == Region.TokenSlot.State.Hidden) {
						hiddenTokens.Add (p);
					}
				}

				foreach (Region.TokenSlot c in r.controlTokens) {

					if (c.m_state == Region.TokenSlot.State.Hidden) {
						hiddenTokens.Add (c);
					}
				}

				if (hiddenTokens.Count > 0) {
					Region.TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
//				tB.ChangeState (TokenBase.State.Revealed);
					tB.m_state = Region.TokenSlot.State.Revealed;
					TokenBase b = tB.GetBaseToken ();
					revealedTokens.Add (tB);
				
				}

			}

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			foreach (Region.TokenSlot ts in revealedTokens) {
				TokenBase tb = ts.GetBaseToken ();
				t.m_resultsText += "\n" + tb.m_name.ToUpper () + " is revealed!";
			}
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
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;

			foreach (Region.TokenSlot a in r.assetTokens) {

				if (a.m_state == Region.TokenSlot.State.Hidden) {
					return true;
				}
			}

			foreach (Region.TokenSlot p in r.policyTokens) {

				if (p.m_state == Region.TokenSlot.State.Hidden) {
					return true;
				}
			}

			foreach (Region.TokenSlot c in r.controlTokens) {

				if (c.m_state == Region.TokenSlot.State.Hidden) {
					return true;
				}
			}
		}

		return false;
	}
}
