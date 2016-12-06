using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionPhaseState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting Mission Phase");

		Organization player = GameManager.instance.game.player;

		List<MissionWrapper> completedMissions = new List<MissionWrapper> ();

		foreach (MissionWrapper a in player.activeMissions) {
			GameManager.instance.currentlyExecutingMission = a;
			a.m_turnsPassed++;

			if (a.m_turnsPassed >= a.m_mission.m_numTurns) {
				
				a.m_mission.CompleteMission (a);
				completedMissions.Add (a);

			} else {
				
				TurnResultsEntry entry = new TurnResultsEntry ();
				string s = null;

				s = a.m_henchmen [0].henchmenName + " continues " + a.m_mission.m_name;
				int turnsRemaining = a.m_mission.m_numTurns - a.m_turnsPassed;
				s += "\n(" + turnsRemaining.ToString () + " Turns Remaining)";
				entry.m_resultType = GameEvent.Henchmen_MissionContinued;

				entry.m_resultsText = s;
				player.AddTurnResults (GameManager.instance.game.turnNumber, entry);

			}

			GameManager.instance.currentlyExecutingMission = null;
		}

		while (completedMissions.Count > 0) {
			MissionWrapper a = completedMissions [0];
//			Debug.Log ("Completing mission: " + a.m_mission.m_name);
			completedMissions.RemoveAt (0);
			player.MissionCompleted (a);
		}

		GameManager.instance.ChangeGameState (GameManager.instance.endPlayerPhase);
	}

	public void UpdateState () {

	}

	public void ExitState (){
		Debug.Log ("Exiting Mission Phase");
	}
}
