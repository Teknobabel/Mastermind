using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAIState_FreeRegion : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Free Region");

		// if player owned control tokens in region, attempt to seize

		List<TokenSlot> validTokenSlots = new List<TokenSlot> ();

		foreach (TokenSlot ts in aw.m_agent.currentRegion.controlTokens) {

			if (ts.owner == Region.Owner.Player)
			{
				validTokenSlots.Add (ts);
			}
		}

		if (validTokenSlots.Count > 0) {

			TokenSlot targetSlot = validTokenSlots[Random.Range(0, validTokenSlots.Count)];

			// attempt to reclaim control token

			Debug.Log ("Attempt to liberate Control Token");

			MissionWrapper mw = new MissionWrapper ();
			mw.m_mission = GameManager.instance.m_agentMissionBank[2];
			mw.m_agents.Add (aw);
			mw.m_agentInFocus = aw;
			mw.m_region = aw.m_agent.currentRegion;
			mw.m_organization = GameManager.instance.game.agentOrganization;

			GameManager.instance.currentMissionWrapper = mw;
			GameManager.instance.ProcessMissionWrapper ();

			return;
		}

		// if not in a region w player owned control tokens, look for a new region

		// prefer regions controlled by player

		List<Region> playerControlled = new List<Region> ();
		List<Region> playerOwned = new List<Region> ();

		Debug.Log ("Looking for Regions with Control Tokens to Seize");

		foreach (Region r in GameManager.instance.game.regions) {

			int playerTokens = 0;

			foreach (TokenSlot ts in r.controlTokens) {

				if (ts.owner == Region.Owner.Player)
				{
					playerTokens ++;
				}
			}

			if (playerTokens == r.controlTokens.Count && playerTokens > 0) {

				playerOwned.Add (r);
			} else if (playerTokens > 0) {

				playerControlled.Add (r);
			}
		}

		Region destination = null;

		if (playerOwned.Count > 0) {

			destination = playerOwned[Random.Range(0, playerOwned.Count)];

		} else if (playerControlled.Count > 0) {

			destination = playerControlled[Random.Range(0, playerControlled.Count)];
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
		}
	}

	public void EnterState (AgentWrapper aw)
	{

	}

	public void ExitState (AgentWrapper aw)
	{

	}
}

