using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentAIState_Idle : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Idle");

		if (!aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.RegionSearched)) {

			// search region

			Debug.Log ("Attempting to Search Region");

			MissionWrapper mw = new MissionWrapper ();
			mw.m_mission = GameManager.instance.m_agentMissionBank [0];
			mw.m_agents.Add (aw);
			mw.m_agentInFocus = aw;
			mw.m_region = aw.m_agent.currentRegion;
			mw.m_organization = GameManager.instance.game.agentOrganization;

			GameManager.instance.currentMissionWrapper = mw;
			GameManager.instance.ProcessMissionWrapper ();

		} else {

			if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.BaseFound)) {

				// invade base

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank[1];
//				mw.m_henchmen.Add (aw.m_agent);
//				mw.m_henchmenInFocus = aw.m_agent;

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

//				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();
//				Debug.Log (GameManager.instance.game.agentOrganization.activeMissions.Count);
			} else if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.HenchmenFound)) {

				// engage henchmen

				Debug.Log ("Attempting to engage Henchmen");

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank[3];
				mw.m_agents.Add (aw);
				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

			} else if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.PlayerControlTokenFound)) {

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

			} else {

				// attempt to hide if currently visible and not tracked

				Debug.Log ("Attempting to hide");

				if (aw.m_vizState == AgentWrapper.VisibilityState.Visible) {

					MissionWrapper mw = new MissionWrapper ();
					mw.m_mission = GameManager.instance.m_agentMissionBank[4];
					mw.m_agentInFocus = aw;
					mw.m_region = aw.m_agent.currentRegion;
					mw.m_organization = GameManager.instance.game.agentOrganization;

					GameManager.instance.currentMissionWrapper = mw;
					GameManager.instance.ProcessMissionWrapper ();

					return;
				}

				// chance to move to new region

				if (Random.Range (0.0f, 1.0f) > 0.4f) {

					// move to new region

					Debug.Log ("Agent moving to new region");

					Region r = GameManager.instance.game.GetRandomRegion (false);

					if (r != null) {

						// choose random slot in region
						List<Region.HenchmenSlot> hsList = new List<Region.HenchmenSlot>();
						foreach (Region.HenchmenSlot hs in r.henchmenSlots)
						{
							if (hs.m_state == Region.HenchmenSlot.State.Empty || hs.m_state == Region.HenchmenSlot.State.Occupied_Player) {

								hsList.Add (hs);
							}
						}
							
						if (hsList.Count > 0) {

							MissionWrapper mw = new MissionWrapper ();
							mw.m_mission = GameManager.instance.m_travelMission;
							mw.m_agents.Add (aw);
							mw.m_agentInFocus = aw;
							mw.m_region = r;
							mw.m_organization = GameManager.instance.game.agentOrganization;

							mw.m_henchmenSlotInFocus = hsList[Random.Range(0, hsList.Count)];

							GameManager.instance.currentMissionWrapper = mw;
							GameManager.instance.ProcessMissionWrapper ();
						}
					}

					// clear out current events

					aw.m_agentEvents.Clear ();
				}
			}
		}
	}
}
