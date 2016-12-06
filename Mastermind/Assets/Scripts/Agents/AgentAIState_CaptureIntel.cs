using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAIState_CaptureIntel : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Capture Intel");

		// capture intel if it exists

		foreach (TokenSlot ts in aw.m_agent.currentRegion.assetTokens) {

			if (ts.m_assetToken != null && ts.m_assetToken == GameManager.instance.m_intel) {

				Debug.Log ("Attempting to capture intel");

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank [6];
				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

				return;
			}
		}

		// if no intel in region, move to region with intel (check hidden)

		List<Region> validRegions = new List<Region> ();

		foreach (Region r in GameManager.instance.game.regions) {

			foreach (TokenSlot ts in r.assetTokens) {

				if (ts.m_assetToken != null && ts.m_assetToken == GameManager.instance.m_intel) {

					validRegions.Add (r);
				}
			}
		}

		// move to region if it exists

		if (validRegions.Count > 0) {

			Region destination = validRegions [Random.Range (0, validRegions.Count)];

			List<Region.HenchmenSlot> validSlots = new List<Region.HenchmenSlot> ();

			foreach (Region.HenchmenSlot hs in destination.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

					validSlots.Add (hs);
				} else if (hs.m_state == Region.HenchmenSlot.State.Empty) {

					bool agentIncoming = false;

					foreach (Henchmen h in hs.m_enRoute) {

						if (h.owner == Region.Owner.AI) {

							agentIncoming = true;
							break;
						}
					}

					if (!agentIncoming) {

						validSlots.Add (hs);
					}
				}
			}

			MissionWrapper mw = new MissionWrapper ();
			mw.m_mission = GameManager.instance.m_travelMission;
			mw.m_agents.Add (aw);
			mw.m_agentInFocus = aw;
			mw.m_region = destination;
			mw.m_henchmenSlotInFocus = validSlots[Random.Range(0, validSlots.Count)];
			mw.m_organization = GameManager.instance.game.agentOrganization;

			GameManager.instance.currentMissionWrapper = mw;
			GameManager.instance.ProcessMissionWrapper ();

			return;

		} else {

			// or else revert to idle state

			aw.ChangeAIState (GameManager.instance.game.agentOrganization.agentAIState_Idle);

			return;
		}
	}

	public void EnterState (AgentWrapper aw)
	{

	}

	public void ExitState (AgentWrapper aw)
	{

	}
}
