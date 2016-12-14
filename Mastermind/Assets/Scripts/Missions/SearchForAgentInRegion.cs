using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Missions/Search For Agent In Region")]
public class SearchForAgentInRegion : MissionBase {

	public override void CompleteMission (MissionWrapper a)
	{
		base.CompleteMission (a);

		TurnResultsEntry t = new TurnResultsEntry ();
		if (a.m_henchmenInFocus != null) {t.m_henchmenIDs.Add (a.m_henchmenInFocus.id);}

		if (a.m_success) {

			Region r = a.m_region;
			List<Henchmen> l = new List<Henchmen> ();

			foreach (Region.HenchmenSlot hs in r.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_vizState == AgentWrapper.VisibilityState.Hidden) {

					hs.m_agent.ChangeVisibilityState (AgentWrapper.VisibilityState.Visible);
					l.Add (hs.m_agent.m_agent);
				}
			}

			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission complete!\n";
			if (l.Count == 0) {
				t.m_resultsText += "No Agents found";
			} else {
				foreach (Henchmen h in l) {

					t.m_resultsText += h.henchmenName.ToUpper () + " discovered in " + r.regionName.ToUpper () + "!\n";
				}
			}
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);

		} else {

			t.m_resultsText = a.m_mission.m_name.ToUpper () + ": " + a.m_region.regionName.ToUpper() + " mission complete!\n";
			t.m_resultsText += "No Agents found";
			GameManager.instance.game.player.AddTurnResults (GameManager.instance.game.turnNumber, t);
		}
	}

	public override bool IsValid ()
	{

		if (GameManager.instance.currentMissionWrapper != null && GameManager.instance.currentMissionWrapper.m_region != null &&
			GameManager.instance.currentMissionWrapper.m_scope == TargetType.Region) {

			Region r = GameManager.instance.currentMissionWrapper.m_region;
			bool emptySlots = false;

			foreach (Region.HenchmenSlot hs in r.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Empty || (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent && hs.m_agent.m_vizState == AgentWrapper.VisibilityState.Hidden)) {

					emptySlots = true;
					break;
				}
			}

			if (emptySlots) {
				return true;
			}
		}
		return false;
	}
}
