using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Agent Phase");

		// do AI for any agents not already on a mission

		foreach (AgentWrapper aw in GameManager.instance.game.agentOrganization.currentAgents) {

			if (aw.m_currentAIState != null && aw.m_agent.currentState != Henchmen.state.OnMission && aw.m_agent.currentState != Henchmen.state.Captured) {

				aw.m_currentAIState.DoAgentTurn (aw);

			} else if (aw.m_agent.currentState != Henchmen.state.Captured) {

				// agent attempts to break out of jail
			}

		}

		// resolve any active missions

		List<MissionWrapper> completedMissions = new List<MissionWrapper> ();

		foreach (MissionWrapper mw in GameManager.instance.game.agentOrganization.activeMissions) {

			GameManager.instance.currentlyExecutingMission = mw;
			mw.m_turnsPassed++;

			if (mw.m_turnsPassed >= mw.m_mission.m_numTurns) {

				mw.m_mission.CompleteMission (mw);
				completedMissions.Add (mw);
			}

			GameManager.instance.currentlyExecutingMission = null;
		}

		// resolve any completed missions

		while (completedMissions.Count > 0) {
			MissionWrapper a = completedMissions [0];
			completedMissions.RemoveAt (0);
			GameManager.instance.game.agentOrganization.MissionCompleted (a);
		}

		// spawn any new agents if needed

		if (GameManager.instance.game.agentOrganization.agentsToSpawn > 0) {
			
			for (int i = 0; i < GameManager.instance.game.agentOrganization.agentsToSpawn; i++) {

				GameManager.instance.game.agentOrganization.SpawnAgentInWorld (null);
			}

			GameManager.instance.game.agentOrganization.agentsToSpawn = 0;
		}

		// go to next turn phase

		GameManager.instance.ChangeGameState (GameManager.instance.endTurnPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Agent Phase");
	}

//	private void MoveToRandomRegion (AgentWrapper aw)
//	{
//		// select a region to place agent in
//
//		List<Region> emptyRegions = new List<Region> ();
//		List<Region> validRegions = new List<Region> ();
//
//		foreach (Region region in GameManager.instance.game.regions) {
//
//			if (region.id != GameManager.instance.game.player.homeRegion.id) {
//
//				if (region.currentHenchmen.Count == 0) {
//
//					emptyRegions.Add (region);
//				} else if (region.currentHenchmen.Count < region.henchmenSlots.Count) {
//
//					validRegions.Add (region);
//				}
//			}
//		}
//
//		Region randRegion = null;
//
//		if (emptyRegions.Count > 0) {
//
//			randRegion = emptyRegions[Random.Range(0, emptyRegions.Count)];
//
//		} else if (validRegions.Count > 0) {
//
//			randRegion = validRegions[Random.Range(0, validRegions.Count)];
//		}
//
//		if (randRegion != null) {
//
//			MissionWrapper mw = new MissionWrapper ();
//			mw.m_mission = GameManager.instance.m_travelMission;
//			mw.m_henchmen.Add (aw.m_agent);
//			mw.m_region = randRegion;
//			mw.m_organization = GameManager.instance.game.agentOrganization;
//
//			GameManager.instance.currentMissionWrapper = mw;
//			GameManager.instance.ProcessMissionWrapper ();
//		}
//	}
}
