using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu()]
public class UncoverToken : MissionBase {

	public override void CompleteMission (Organization.ActiveMission a)
	{
		base.CompleteMission (a);

		// determine completion percentage

		int completionPercentage = 0;

		List<TraitData> combinedTraitList = new List<TraitData> ();

		foreach (Henchmen thisH in a.m_henchmen) {
			List<TraitData> t = thisH.GetAllTraits();
			foreach (TraitData thisT in t) {
				if (!combinedTraitList.Contains (thisT)) {
					combinedTraitList.Add (thisT);
				}
			}
		}

		MissionBase.MissionTrait[] traits = a.m_mission.GetTraitList (1);

		foreach (MissionBase.MissionTrait mt in traits) {
			if (mt.m_trait != null && combinedTraitList.Contains (mt.m_trait)) {
				completionPercentage += mt.m_percentageContribution;
			}
		}


		bool missionSuccess = false;
		int rand = Random.Range (0, 101);
		if (rand <= completionPercentage) {
			
			missionSuccess = true;
		}

		if (missionSuccess) {

			// gather all hidden tokens

			Region r = a.m_region;

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

				TurnResultsEntry t = new TurnResultsEntry ();
				t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission is a success!";
				t.m_resultsText += "\n" + b.m_name.ToUpper() + " is revealed!";
				t.m_resultsText += "\n" + rand.ToString () + " / " + completionPercentage.ToString ();
				t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
				t.m_resultType = GameEvent.Henchmen_MissionCompleted;
				GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
			}
		} else {

			TurnResultsEntry t = new TurnResultsEntry ();
			t.m_resultsText = a.m_mission.m_name.ToUpper () + " mission fails.";
			t.m_resultsText += "\n" + rand.ToString () + " / " + completionPercentage.ToString ();
			t.m_resultsText += "\n +" + a.m_mission.m_infamyGain.ToString () + " Infamy";
			t.m_resultType = GameEvent.Henchmen_MissionCompleted;
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		}

	}

	public override bool IsValid ()
	{
		// valid if there are any hidden tokens in the region

		if (GameManager.instance.currentMissionRequest != null && GameManager.instance.currentMissionRequest.m_region != null) {

			Region r = GameManager.instance.currentMissionRequest.m_region;

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
