using UnityEngine;
using System.Collections;

public class AgentAIState_Idle : IAgentAIState {

	public void DoAgentTurn (AgentWrapper aw)
	{
		Debug.Log ("Starting Agent AI State: Idle");


		if (!aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.RegionSearched)) {

			// search region

			MissionWrapper mw = new MissionWrapper ();
			mw.m_mission = GameManager.instance.m_agentMissionBank [0];
			mw.m_henchmen.Add (aw.m_agent);
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
				mw.m_henchmen.Add (aw.m_agent);
				mw.m_henchmenInFocus = aw.m_agent;
				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

			} else if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.HenchmenFound)) {

				// engage henchmen

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank[3];
				mw.m_henchmen.Add (aw.m_agent);
				mw.m_henchmenInFocus = aw.m_agent;
				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

			} else if (aw.m_agentEvents.Contains (AgentWrapper.AgentEvents.PlayerControlTokenFound)) {

				// attempt to reclaim control token

				MissionWrapper mw = new MissionWrapper ();
				mw.m_mission = GameManager.instance.m_agentMissionBank[2];
				mw.m_henchmen.Add (aw.m_agent);
				mw.m_henchmenInFocus = aw.m_agent;
				mw.m_agentInFocus = aw;
				mw.m_region = aw.m_agent.currentRegion;
				mw.m_organization = GameManager.instance.game.agentOrganization;

				GameManager.instance.currentMissionWrapper = mw;
				GameManager.instance.ProcessMissionWrapper ();

			} else {

				// chance to move to new region

				if (Random.Range (0.0f, 1.0f) > 0.4f) {

					// move to new region

					Debug.Log ("Agent moving to new region");

					Region r = GameManager.instance.game.GetRandomRegion (false);

					if (r != null) {

						MissionWrapper mw = new MissionWrapper ();
						mw.m_mission = GameManager.instance.m_travelMission;
						mw.m_henchmen.Add (aw.m_agent);
						mw.m_henchmenInFocus = aw.m_agent;
						mw.m_agentInFocus = aw;
						mw.m_region = r;
						mw.m_organization = GameManager.instance.game.agentOrganization;

						GameManager.instance.currentMissionWrapper = mw;
						GameManager.instance.ProcessMissionWrapper ();
					}

					// clear out current events

					aw.m_agentEvents.Clear ();
				}
			}
		}
	}
}
