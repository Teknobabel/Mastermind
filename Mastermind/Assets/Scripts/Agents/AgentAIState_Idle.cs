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

			} else if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.HenchmenFound)) {

				// check for missions in region to disrupt

				foreach (MissionWrapper mw in GameManager.instance.game.player.activeMissions) {

					if (mw.m_region != null && mw.m_region.id == aw.m_agent.currentRegion.id) {

						// attempt to disrupt mission

						Debug.Log ("Attempting to disrupt Henchmen mission");

						MissionWrapper mw2 = new MissionWrapper ();
						mw2.m_mission = GameManager.instance.m_agentMissionBank [5];
						mw2.m_agentInFocus = aw;
						mw2.m_region = aw.m_agent.currentRegion;
						mw2.m_organization = GameManager.instance.game.agentOrganization;

						GameManager.instance.currentMissionWrapper = mw2;
						GameManager.instance.ProcessMissionWrapper ();

						return;
					}
				}

				// if no active missions, engage henchmen directly

				// make sure there are still henchmen here

				if (aw.m_agent.currentRegion.currentHenchmen.Count > 0) {

					Debug.Log ("Attempting to engage Henchmen");

					MissionWrapper mw = new MissionWrapper ();
					mw.m_mission = GameManager.instance.m_agentMissionBank [3];
//					mw.m_agentInFocus = aw;
					foreach (Region.HenchmenSlot hs in aw.m_agent.currentRegion.henchmenSlots) {

						if (hs.m_state == Region.HenchmenSlot.State.Occupied_Agent) {

							if (hs.m_agent.m_agent.currentState == Henchmen.state.OnMission) {

								GameManager.instance.game.agentOrganization.RemoveAgentFromMissions (hs.m_agent);
							}

							mw.m_agents.Add (hs.m_agent);
						}
					}
					mw.m_region = aw.m_agent.currentRegion;
					mw.m_organization = GameManager.instance.game.agentOrganization;

					GameManager.instance.currentMissionWrapper = mw;
					GameManager.instance.ProcessMissionWrapper ();

				} else {

					aw.m_agentEvents.Remove (AgentWrapper.AgentEvents.HenchmenFound);
				}

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
