using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAIState_AttackHenchmen : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Attack Henchmen");

		// revert back to idle state after a few turns

		if (GameManager.instance.game.turnNumber - aw.m_turnEnteredState >= 5) {

			aw.ChangeAIState (GameManager.instance.game.agentOrganization.agentAIState_Idle);
			return;
		}

		// if in region w henchmen, attack

		foreach (Region.HenchmenSlot hs in aw.m_agent.currentRegion.henchmenSlots) {

			if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank [3];
				//					mw.m_agentInFocus = aw;
				foreach (Region.HenchmenSlot hs2 in aw.m_agent.currentRegion.henchmenSlots) {

					if (hs2.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

						if (hs2.m_agent.m_agent.currentState == Henchmen.state.OnMission) {

							GameManager.instance.game.agentOrganization.RemoveAgentFromMissions (hs2.m_agent);
						}

						mw.m_agents.Add (hs2.m_agent);
					}
				}

				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

				return;
			}
		}

		// else move to a region with henchmen

		// prefer regions with agents (incoming or present) and henchmen

		List<Region> agentsPresent = new List<Region> ();
		List<Region> noAgentsPresent = new List<Region> ();

		Region destination = null;
		Region.HenchmenSlot slot = null;

		foreach (Region r in GameManager.instance.game.regions) {

			bool agents = false;
			bool henchmen = false;
			foreach (Region.HenchmenSlot hs in r.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

					agents = true;
				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {
					henchmen = true;
				}
			}

			if (agents && henchmen) {

				agentsPresent.Add (r);
			} else if (!agents && henchmen) {
				noAgentsPresent.Add (r);
			}
		}

		// prefer empty slot, else ambush

		if (agentsPresent.Count > 0) {

			destination = agentsPresent[Random.Range(0, agentsPresent.Count)];
		} else if (noAgentsPresent.Count > 0) {
			destination = noAgentsPresent[Random.Range(0, noAgentsPresent.Count)];
		}

		if (destination != null) {

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

			if (validSlots.Count > 0) {

				slot = validSlots[Random.Range(0, validSlots.Count)];

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_travelMission;
				mw.m_agents.Add (aw);
				mw.m_agentInFocus = aw;
				mw.m_region = destination;
				mw.m_henchmenSlotInFocus = slot;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

				return;
			}
		}
	}

	public void EnterState (AgentWrapper aw)
	{
		aw.m_turnEnteredState = GameManager.instance.game.turnNumber;
	}

	public void ExitState (AgentWrapper aw)
	{
		
	}
}
