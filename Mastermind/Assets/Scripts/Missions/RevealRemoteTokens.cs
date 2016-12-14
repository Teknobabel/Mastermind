using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Reveal Remote Tokens")]
public class RevealRemoteTokens : MissionBase {

	public int m_numTokens = 1;

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);
//		a.m_success = true;

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			// gather all hidden tokens

			Region r = a.m_regionInFocus;

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
				Debug.Log (hiddenTokens.Count);
				if (hiddenTokens.Count > 0) {
					TokenSlot tB = hiddenTokens [Random.Range (0, hiddenTokens.Count)];
					tB.m_state = TokenSlot.State.Revealed;
					revealedTokens.Add (tB);

				}
			}

			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
			foreach (TokenSlot ts in revealedTokens) {
				

					TokenBase tb = ts.GetBaseToken ();
				if (tb != null) {
					t.m_resultsText += "\n" + tb.m_name.ToUpper () + " is revealed!";
				} else {
					t.m_resultsText += "\nEmpty " + ts.m_type.ToString().ToUpper () + " is revealed!";
				}

			}
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

	public override string GetNameText ()
	{
		string s = m_name + "\n";

		if (GameManager.instance.currentMissionWrapper.m_regionInFocus != null) {
			s += "<size=18>" + GameManager.instance.currentMissionWrapper.m_regionInFocus.regionName.ToUpper() + "</size>";
		}

		return s;
	}

	public override bool IsValid ()
	{
		MissionWrapper mw = GameManager.instance.currentMissionWrapper;

		if (mw.m_scope == TargetType.Floor && mw.m_floorInFocus.m_installedUpgrade.m_assetType == Asset.AssetType.LairUpgrade_ControlRoom &&
			mw.m_regionInFocus.id != GameManager.instance.game.player.homeRegion.id) {

			return true;
		}
		return false;
	}
}
