using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAIState_AttackBase : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Attack Base");

		// if not at base, move to region with base

		if (aw.m_agent.currentRegion.id != GameManager.instance.game.player.homeRegion.id) {

			List< Region.HenchmenSlot> emptySlots = new List<Region.HenchmenSlot> ();
			List< Region.HenchmenSlot> ambushSlot = new List<Region.HenchmenSlot> ();

			Region.HenchmenSlot slotInFocus = null;

			Region baseRegion = GameManager.instance.game.player.homeRegion;

			foreach (Region.HenchmenSlot hs in baseRegion.henchmenSlots) {

				if (hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

					ambushSlot.Add (hs);
				} else if (hs.m_state == Region.HenchmenSlot.State.Empty) {

					// make sure no agents reserved slot
					bool agentInbound = false;

					foreach (Henchmen h in hs.m_enRoute) {
						if (h.owner == Region.Owner.AI) {
							agentInbound = true;
						}
					}

					if (!agentInbound) {

						emptySlots.Add (hs);
					}
				}
			}

			if (emptySlots.Count > 0) {

				slotInFocus = emptySlots[Random.Range(0, emptySlots.Count)];
			} else if (ambushSlot.Count > 0) {

				slotInFocus = ambushSlot[Random.Range(0, ambushSlot.Count)];

			}

			if (slotInFocus != null)
			{
				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_travelMission;
				mw.m_agents.Add (aw);
				mw.m_agentInFocus = aw;
				mw.m_region = baseRegion;
				mw.m_henchmenSlotInFocus = slotInFocus;
				mw.m_organization = GameManager.instance.game.agentOrganization;
	
				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

				return;
			}

		} else {

			// if at base and henchmen are present, attack henchmen

			int numAgents = 0;

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

				} else if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

					numAgents++;
				}
			}

			// if at base w no henchmen and all attackers present, attack base

			if (numAgents == aw.m_agent.currentRegion.henchmenSlots.Count || GameManager.instance.game.agentOrganization.agentsByState[this].Count == numAgents) {

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank [1];

				//add all agents in region, cancelling any missions they are currently on

				foreach (Region.HenchmenSlot hs in aw.m_agent.currentRegion.henchmenSlots) {

					if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

						if (hs.m_agent.m_agent.currentState == Henchmen.state.OnMission) {

							// remove from current mission, cancel if empty

							GameManager.instance.game.agentOrganization.RemoveAgentFromMissions (hs.m_agent);

						}

						mw.m_agents.Add (hs.m_agent);
					}
				}

				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

				return;

			}


			// else, wait for all base attackers to show up

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
