using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Agent Phase");

		List<MissionWrapper> missions = new List<MissionWrapper> ();

		foreach (AgentWrapper aw in GameManager.instance.game.agentsInPlay) {

			if (Random.Range (0.0f, 1.0f) > 0.65f) {
				missions.Add (MoveToRandomRegion (aw));
			}
		
		}

		foreach (MissionWrapper mw in missions) {

			mw.m_turnsPassed++;

			if (mw.m_turnsPassed >= mw.m_mission.m_numTurns) {

				mw.m_mission.CompleteMission (mw);

			}
		}

		GameManager.instance.ChangeGameState (GameManager.instance.endTurnPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Agent Phase");
	}

	private MissionWrapper MoveToRandomRegion (AgentWrapper aw)
	{
		// select a region to place agent in

		List<Region> emptyRegions = new List<Region> ();
		List<Region> validRegions = new List<Region> ();

		foreach (Region region in GameManager.instance.game.regions) {

			if (region.id != GameManager.instance.game.player.homeRegion.id) {

				if (region.currentHenchmen.Count == 0) {

					emptyRegions.Add (region);
				} else if (region.currentHenchmen.Count < region.henchmenSlots.Count) {

					validRegions.Add (region);
				}
			}
		}

		Region randRegion = null;

		if (emptyRegions.Count > 0) {

			randRegion = emptyRegions[Random.Range(0, emptyRegions.Count)];

		} else if (validRegions.Count > 0) {

			randRegion = validRegions[Random.Range(0, validRegions.Count)];
		}

		if (randRegion != null) {

			MissionWrapper mw = new MissionWrapper ();
			mw.m_mission = GameManager.instance.m_travelMission;
			mw.m_henchmen.Add (aw.m_agent);
			mw.m_region = randRegion;

			return mw;
		}

		return null;
	}
}
